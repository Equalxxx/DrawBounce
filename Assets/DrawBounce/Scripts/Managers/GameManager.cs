using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLFramework;

public enum GameState { GameTitle, GamePlay, GameOver, EnterShop }

[System.Serializable]
public class GameInfo
{
	public string userId;
	public int coin;
	public int playerHP;
	public int playerMaxHP;
	public float startHeight;
	public float lastHeight;
}

public class GameManager : Singleton<GameManager>
{
	[Header("Game Play Info")]
    public GameState gameState;
	public GameInfo gameInfo;

	[Header("GameLevel Info")]
	public TargetHeightInfo curTargetHeight;
	public int maxLevel;

	public float getCoinHeight = 30f;
	public float maxStartHeight = 1000f;
	public float limitStartHeight = 100f;
	public float moveToDuration = 5f;
	public int lastStartHeight;

	public PlayableBlockType curBlockType;

	public static bool IsPause;
	public static bool IsNoAds;
	public static bool IsOfflineMode;
	public static bool IsConnected
	{
		get
		{
			if (!GooglePlayManager.IsAuthenticated || Application.internetReachability == NetworkReachability.NotReachable)
			{
				if(!IsOfflineMode)
				{
					IsOfflineMode = true;
					UIManager.Instance.ShowOfflineMode(true);
				}

				return false;
			}
			else
			{
				return true;
			}
		}
	}

	// Game State Actions
	public static Action GameInitAction;
    public static Action GamePlayAction;
	public static Action GameOverAction;

	public static Action<float> SetStartHeightAction;
	public static Action PauseAction;

	public static Action<PlayableBlockType> SetPlayableBlockAction;
	public static Action<int> SetBGAction;

	[Header("GameManager Settings")]
	public GameSettings gameSettings;
	public DeviceSettings deviceSettings;
	[HideInInspector]
    public PlayableBlock curPlayableBlock;
	public Transform playableBlockParent;
	public BGControl bgControl;

	[Header("DataTables")]
	public GameDataTable gameDataTable;
	public MapDataTable mapDataTable;

	private void Start()
    {
		InitGame();

        FadeScreen.Instance.StartFade(false);

		SoundManager.Instance.PlayMusic("BGM_1", true);
	}

	void InitGame()
	{
#if !UNITY_EDITOR
		if(Application.systemLanguage == SystemLanguage.Korean)
		{
			LocalizeManager.Instance.SetLanguage(LocalizeManager.LocalizeLanguageType.KR);
		}
		else if(Application.systemLanguage == SystemLanguage.Japanese)
		{
			LocalizeManager.Instance.SetLanguage(LocalizeManager.LocalizeLanguageType.JP);
		}
		else
		{
			LocalizeManager.Instance.SetLanguage(LocalizeManager.LocalizeLanguageType.ENG);
		}
#endif

		CheckNoAds();

		//gameSettings.LoadDeviceOptions();
		gameSettings.LoadGameData();

		gameState = GameState.GameTitle;

		StartCoroutine(GameLoop());

		Invoke("CheckTutorial", 1f);
	}

	public void CheckNoAds()
	{
		if (IsConnected)
			IAPManager.Instance.ProductValidate("noadspackage", SetNoAds);
		else
			SetNoAds(true);
	}

	void CheckTutorial()
	{
		if (deviceSettings.tutorial)
		{
			Debug.Log("Show Tutorial");
			UIManager.Instance.ShowPopup(PopupUIType.Tutorial, true);
			deviceSettings.tutorial = false;
			gameSettings.SaveDeviceOptions();
		}
	}

	void SetNoAds(bool noAds)
	{
		IsNoAds = noAds;

		Debug.LogFormat("NoAds : {0}", IsNoAds);

		//if (IsNoAds)
		//{
		//	AdmobManager.Instance.HideBannerAd();
		//}
		//else
		//{
		//	AdmobManager.Instance.ShowAd(AdmobAdType.Banner);
		//}

		UIManager.Instance.ShowNoAdsButton(!IsNoAds);

		UIManager.Instance.SetUIRects();
	}

	private void OnApplicationPause(bool pause)
	{
		if (gameState != GameState.GamePlay)
			return;

		if(pause)
		{
			SetPause(pause);
		}
	}

	IEnumerator GameLoop()
    {
        while(true)
        {
			switch(gameState)
			{
				case GameState.GameTitle:
					yield return GameTitle();
					break;
				case GameState.GamePlay:
					yield return GamePlay();
					break;
				case GameState.GameOver:
					yield return GameOver();
					break;
				case GameState.EnterShop:
					yield return EnterShop();
					break;
			}
        }
    }

    IEnumerator GameTitle()
    {
        Debug.Log("Title menu!");
        UIManager.Instance.ShowUIGroup(UIGroupType.Title);

		curTargetHeight = gameDataTable.GetTargetHeightInfo(1);

		curPlayableBlock.InitPlayer();

		GameInitAction?.Invoke();
		lastStartHeight = 0;

		while (gameState == GameState.GameTitle)
        {
            yield return null;
        }

		Debug.Log("Title menu is done!");
    }

    IEnumerator GamePlay()
    {
		Debug.Log("Game play!");
        UIManager.Instance.ShowUIGroup(UIGroupType.Game);

		curPlayableBlock.HP = gameInfo.playerHP;

		curTargetHeight = gameDataTable.GetTargetHeightInfo(gameInfo.startHeight);

		SetStartHeightAction?.Invoke(gameInfo.startHeight);

		GamePlayAction?.Invoke();


		while (gameState == GameState.GamePlay)
        {
			if(!curPlayableBlock.isFastMove)
			{
				SetLevel(curPlayableBlock.height);
			}

            yield return null;
        }

		yield return new WaitForSeconds(0.5f);

		lastStartHeight = (int)gameInfo.startHeight;

		if(curPlayableBlock.blockType != PlayableBlockType.CoinCircle)
			gameInfo.playerHP = 1;

		gameInfo.startHeight = 0f;

		yield return WaittingSaving();

		Debug.Log("Game play is done!");
	}

    IEnumerator GameOver()
    {
        Debug.Log("Game over!");

		if(IsConnected)
		{
			GooglePlayManager.Instance.ReportScore((int)gameInfo.lastHeight);
			UnlockAchievement((int)gameInfo.lastHeight);
		}

        UIManager.Instance.ShowUIGroup(UIGroupType.Result);

		GameOverAction?.Invoke();

        while (gameState == GameState.GameOver)
        {
            yield return null;
        }

		Debug.Log("Game over is done!");
	}

	IEnumerator EnterShop()
	{
		Debug.Log("Enter shop!");
		UIManager.Instance.ShowUIGroup(UIGroupType.Shop);
		curPlayableBlock.Show(false);

		while (gameState == GameState.EnterShop)
		{
			yield return null;
		}

		Debug.Log("Shop is done!");
	}

	public void SaveGame()
	{
		StartCoroutine(WaittingSaving());
	}

	IEnumerator WaittingSaving()
	{
		gameSettings.SaveGameData();

		while (gameSettings.isProcessing)
		{
			yield return null;
		}

		Debug.Log("Save is done");
	}

	void UnlockAchievement(int height)
	{
		if (height >= 2000)
		{
			GooglePlayManager.Instance.UnlockAchievement(GPGSIds.achievement_2000);
		}
		if (height >= 1000)
		{
			GooglePlayManager.Instance.UnlockAchievement(GPGSIds.achievement_1000);
		}
		if (height >= 900)
		{
			GooglePlayManager.Instance.UnlockAchievement(GPGSIds.achievement_900);
		}
		if (height >= 700)
		{
			GooglePlayManager.Instance.UnlockAchievement(GPGSIds.achievement_700);
		}
		if (height >= 500)
		{
			GooglePlayManager.Instance.UnlockAchievement(GPGSIds.achievement_500);
		}
		if (height >= 300)
		{
			GooglePlayManager.Instance.UnlockAchievement(GPGSIds.achievement_300);
		}
		if (height >= 100)
		{
			GooglePlayManager.Instance.UnlockAchievement(GPGSIds.achievement_100);
		}
	}

	void SetLevel(float height)
	{
		if (height >= curTargetHeight.targetHeight)
		{
			int nextLevel = curTargetHeight.level + 1;
			if (nextLevel > maxLevel)
				return;

			curTargetHeight = gameDataTable.GetTargetHeightInfo(nextLevel);
		}
	}

	public int GetHeightCoinValue()
	{
		return (int)(10f * curTargetHeight.level * (curPlayableBlock.addHeightCoinPer/100f));
	}

	public bool IsAddCoin(int addCoin)
	{
		if (gameInfo.coin + addCoin > int.MaxValue)
		{
			Debug.LogWarningFormat("Coin value max : {0}", addCoin);
			return false;
		}

		return true;
	}

	public void AddCoin(int addCoin)
    {
		gameInfo.coin += addCoin;

		UIManager.Instance.RefreshCoinInfoUI();

		Debug.LogFormat("Add coin : {0}", addCoin);
    }

	public bool IsUseCoin(int useCoin)
	{
		if (gameInfo.coin < useCoin)
		{
			Debug.LogWarningFormat("Coin not enough : {0}", useCoin);
			return false;
		}
		else
			return true;
	}

	public void UseCoin(int useCoin)
	{
		gameInfo.coin -= useCoin;

		UIManager.Instance.RefreshCoinInfoUI();

		Debug.LogFormat("Used coin : {0}", useCoin);
	}

	public bool IsAddHP(int addHp)
	{
		if (gameInfo.playerHP + addHp > gameInfo.playerMaxHP)
		{
			return false;
		}
		else
			return true;
	}

	public void AddHP(int addHp)
	{
		gameInfo.playerHP += addHp;

		if (gameInfo.playerHP == gameInfo.playerMaxHP)
		{
			SoundManager.Instance.PlaySound2D("Buy_Item_End");
		}
		else
		{
			SoundManager.Instance.PlaySound2D("Buy_Item");
		}

		Debug.LogFormat("Add player hp : {0}", addHp);
	}

	public bool IsAddHeight(float addHeight)
	{
		if (gameInfo.startHeight + addHeight > maxStartHeight || gameInfo.startHeight + addHeight > gameInfo.lastHeight)
		{
			return false;
		}
		else
			return true;
	}

	public void AddHeight(float addHeight)
	{
		gameInfo.startHeight += addHeight;
		int target = (int)(gameInfo.lastHeight / 100f) * 100;

		if (gameInfo.startHeight >= target || gameInfo.startHeight >= maxStartHeight)
		{
			SoundManager.Instance.PlaySound2D("Buy_Item_End");
		}
		else
		{
			SoundManager.Instance.PlaySound2D("Buy_Item");
		}

		Debug.LogFormat("Add start height : {0}", addHeight);
	}

	public void SetSoundSetting(SoundType soundType, bool useSound)
	{
		switch(soundType)
		{
			case SoundType.BGM:
				Debug.LogFormat("Sound use BGM : {0}", useSound);
				deviceSettings.useBGM = useSound;
				if (!deviceSettings.useBGM)
				{
					SoundManager.Instance.bgmCustomVolume = 0f;
				}
				else
				{
					SoundManager.Instance.bgmCustomVolume = 1f;
				}
				break;
			case SoundType.SE:
				Debug.LogFormat("Sound use SE : {0}", useSound);
				deviceSettings.useSE = useSound;
				if (!deviceSettings.useSE)
				{
					SoundManager.Instance.seCustomVolume = 0f;
				}
				else
				{
					SoundManager.Instance.seCustomVolume = 1f;
				}
				break;
		}
	}

	public void SetPause(bool pause)
	{
		IsPause = pause;

		UIManager.Instance.ShowPopup(PopupUIType.Pause, IsPause);

		PauseAction?.Invoke();
	}

	public void SetPlayableBlockType(PlayableBlockType blockType)
	{
		if (curPlayableBlock != null)
		{
			if (curPlayableBlock.blockType == blockType)
				return;

			curPlayableBlock.Show(false);
		}

		curBlockType = blockType;

		GameObject newBlock = null;
		string playableTag = "";

		switch (curBlockType)
		{
			case PlayableBlockType.Circle:
				playableTag = "Circle";
				break;
			case PlayableBlockType.Rectangle:
				if (gameInfo.lastHeight >= 100f)
					playableTag = "Rectangle";
				break;
			case PlayableBlockType.Triangle:
				if (gameInfo.lastHeight >= 100f)
					playableTag = "Triangle";
				break;
			case PlayableBlockType.Star:
				if (gameInfo.lastHeight >= 100f)
					playableTag = "Star";
				break;
			case PlayableBlockType.Heart:
				if (gameInfo.lastHeight >= 100f)
					playableTag = "Heart";
				break;
			case PlayableBlockType.Male:
				if (gameInfo.lastHeight >= 100f)
					playableTag = "Male";
				break;
			case PlayableBlockType.Female:
				if (gameInfo.lastHeight >= 100f)
					playableTag = "Female";
				break;
			case PlayableBlockType.CoinCircle:
				if(IsNoAds)
					playableTag = "CoinCircle";
				break;
		}

		if (string.IsNullOrEmpty(playableTag))
			playableTag = "Circle";

		newBlock = PoolManager.Instance.Spawn(playableTag, Vector3.zero, Quaternion.identity, playableBlockParent);

		curPlayableBlock = newBlock.GetComponent<PlayableBlock>();
		curPlayableBlock.InitPlayer();

		if (gameState == GameState.EnterShop)
			curPlayableBlock.Show(false);

		GameInitAction?.Invoke();
		SetPlayableBlockAction?.Invoke(curBlockType);
	}

	public void SetBGColor(int bgIndex)
	{
		bool changeBG = false;

		if(bgIndex == 5 && gameInfo.lastHeight >= 1000f)
		{
			changeBG = true;
		}
		else if(bgIndex == 4 && gameInfo.lastHeight >= 1000f)
		{
			changeBG = true;
		}
		else if (bgIndex == 3 && gameInfo.lastHeight >= 500f)
		{
			changeBG = true;
		}
		else if (bgIndex == 2 && gameInfo.lastHeight >= 500f)
		{
			changeBG = true;
		}
		else if (bgIndex == 1 && gameInfo.lastHeight >= 300f)
		{
			changeBG = true;
		}

		if (!changeBG)
			bgIndex = 0;

		deviceSettings.bgIndex = bgIndex;
		bgControl.ChangeBGColor(bgIndex);

		SetBGAction?.Invoke(bgIndex);
	}

	public void SetGameState(GameState state)
    {
        gameState = state;
    }
}
