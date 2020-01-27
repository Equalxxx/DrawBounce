using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public enum GameState { GameTitle, GamePlay, GameOver, EnterShop }

[System.Serializable]
public class GameInfo
{
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

	public PlayableBlockType curBlockType;

	[Header("Game Settings")]
	public bool isPause;
	public bool isMuteBGM;
	public bool isMuteSE;
	public bool isVibe;
	public bool isTutorial;
	public static bool IsPracticeMode;

	// Game State Actions
	public static Action GameInitAction;
    public static Action GamePlayAction;
	public static Action GameOverAction;

	// Game Play Actions
	public static Action AddCoinAction;
	public static Action UseCoinAction;
	public static Action AddPlayerHPAction;
	public static Action AddHeightAction;

	public static Action<float> SetStartHeightAction;
	public static Action<bool> PauseAction;

	public static Action SoundMuteAction;
	public static Action ViberateAction;

	public static Action<PlayableBlockType> SetPlayableBlockAction;

	[Header("GameManager Settings")]
	public GameSettings gameSettings;
	public bool testMode;
	[HideInInspector]
    public PlayableBlock player;
	public Transform playerParent;
	public BGControl bgControl;

	[Header("DataTables")]
	public GameDataTable gameDataTable;
	public MapDataTable mapDataTable;

	private void Start()
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
		gameSettings.LoadGameInfo();

        gameState = GameState.GameTitle;

		StartCoroutine(GameLoop());

        FadeScreen.Instance.StartFade(false);

		SoundManager.Instance.PlayMusic("BGM_1", true);
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

		player.InitPlayer();
		bgControl.ChangeBGColor(true);
		GameInitAction?.Invoke();

		yield return new WaitForSeconds(0.25f);
		if (isTutorial)
		{
			UIManager.Instance.tutorialUI.Show(true);
			isTutorial = false;
		}

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

		player.HP = gameInfo.playerHP;

		curTargetHeight = gameDataTable.GetTargetHeightInfo(gameInfo.startHeight);

		SetStartHeightAction?.Invoke(gameInfo.startHeight);

		GamePlayAction?.Invoke();


		while (gameState == GameState.GamePlay)
        {
			if(!player.isFastMove)
			{
				SetLevel(player.height);
			}

            yield return null;
        }

		Debug.Log("Game play is done!");
	}

    IEnumerator GameOver()
    {
        Debug.Log("Game over!");

		yield return new WaitForSeconds(0.5f);

        UIManager.Instance.ShowUIGroup(UIGroupType.Result);

		gameInfo.playerHP = 1;
		gameInfo.startHeight = 0f;

		gameSettings.SaveInfoToServer();

		GooglePlayManager.Instance.ReportScore((int)gameInfo.lastHeight);
		UnlockAchievement((int)gameInfo.lastHeight);

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
		player.Show(false);

		while (gameState == GameState.EnterShop)
		{
			yield return null;
		}

		Debug.Log("Shop is done!");
	}

	public static bool IsConnected
	{
		get
		{
			if (!GooglePlayManager.IsAuthenticated || 
				Application.internetReachability == NetworkReachability.NotReachable)
			{
				IsPracticeMode = true;
				UIManager.Instance.ShowPracticeUI(true);
				return false;
			}
			else
			{
				if(!IsPracticeMode)
					UIManager.Instance.ShowPracticeUI(false);

				return true;
			}
		}
	}

	void UnlockAchievement(int height)
	{
		string achievementId = "";

		if (height >= 2000)
		{
			achievementId = GPGSIds.achievement_2000;
		}
		else if (height >= 1000)
		{
			achievementId = GPGSIds.achievement_1000;
		}
		else if (height >= 900)
		{
			achievementId = GPGSIds.achievement_900;
		}
		else if (height >= 700)
		{
			achievementId = GPGSIds.achievement_700;
		}
		else if (height >= 500)
		{
			achievementId = GPGSIds.achievement_500;
		}
		else if (height >= 300)
		{
			achievementId = GPGSIds.achievement_300;
		}
		else if (height >= 100)
		{
			achievementId = GPGSIds.achievement_100;
		}
		else
		{
			return;
		}

		GooglePlayManager.Instance.UnlockAchievement(achievementId);
	}

	void SetLevel(float height)
	{
		if (height >= curTargetHeight.targetHeight)
		{
			int nextLevel = curTargetHeight.level + 1;
			if (nextLevel > maxLevel)
				return;

			curTargetHeight = gameDataTable.GetTargetHeightInfo(nextLevel);
			bgControl.ChangeBGColor();
		}
	}

	public int GetHeightCoinValue()
	{
		return (int)(10f * curTargetHeight.level * (player.addHeightCoinPer/100f));
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

        AddCoinAction?.Invoke();

		gameSettings.SaveInfoToServer();

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

		UseCoinAction?.Invoke();

		gameSettings.SaveInfoToServer();

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

		AddPlayerHPAction?.Invoke();

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

		AddHeightAction?.Invoke();

		Debug.LogFormat("Add start height : {0}", addHeight);
	}

	public void SetSoundMute(SoundType soundType, bool mute)
	{
		switch(soundType)
		{
			case SoundType.BGM:
				Debug.LogFormat("Sound mute BGM : {0}", mute);
				isMuteBGM = mute;
				if (isMuteBGM)
				{
					SoundManager.Instance.bgmCustomVolume = 0f;
				}
				else
				{
					SoundManager.Instance.bgmCustomVolume = 1f;
				}
				break;
			case SoundType.SE:
				Debug.LogFormat("Sound mute SE : {0}", mute);
				isMuteSE = mute;
				if (isMuteSE)
				{
					SoundManager.Instance.seCustomVolume = 0f;
				}
				else
				{
					SoundManager.Instance.seCustomVolume = 1f;
				}
				break;
		}

		SoundMuteAction?.Invoke();
	}

	public void SetViberate(bool vibe)
	{
		Debug.LogFormat("Viberate : {0}", vibe);

		isVibe = vibe;

		ViberateAction?.Invoke();
	}

	public void SetPause(bool pause)
	{
		isPause = pause;

		if(isPause)
		{
			Time.timeScale = 0f;
		}
		else
		{
			Time.timeScale = 1f;
		}

		PauseAction?.Invoke(isPause);
		UIManager.Instance.ShowPauseUI(isPause);
	}

	public void SetPlayableBlockType(PlayableBlockType blockType)
	{
		if (player != null)
		{
			if (player.blockType == blockType)
				return;

			player.Show(false);
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
				playableTag = "Rectangle";
				break;
			case PlayableBlockType.Triangle:
				playableTag = "Triangle";
				break;
			case PlayableBlockType.Star:
				playableTag = "Star";
				break;
			case PlayableBlockType.Heart:
				playableTag = "Heart";
				break;
			case PlayableBlockType.Male:
				playableTag = "Male";
				break;
			case PlayableBlockType.Female:
				playableTag = "Female";
				break;
		}

		newBlock = PoolManager.Instance.Spawn(playableTag, Vector3.zero, Quaternion.identity, playerParent);

		player = newBlock.GetComponent<PlayableBlock>();
		player.InitPlayer();

		if (gameState == GameState.EnterShop)
			player.Show(false);

		SetPlayableBlockAction?.Invoke(curBlockType);
	}

	public void SetGameState(GameState state)
    {
        gameState = state;
    }
}
