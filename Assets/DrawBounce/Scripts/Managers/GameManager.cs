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
	public int gem;

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

	[Header("Game Settings")]
	public bool isPause;
	public bool isMuteBGM;
	public bool isMuteSE;

	// Game State Actions
	public static Action GameInitAction;
    public static Action GamePlayAction;
	public static Action GameOverAction;

	// Game Play Actions
	public static Action AddGemAction;
	public static Action UseGemAction;
	public static Action AddCoinAction;
	public static Action UseCoinAction;

	public static Action AddPlayerHPAction;
	public static Action AddHeightAction;

	public static Action<float> SetStartHeightAction;
	public static Action<bool> PauseAction;

	public static Action SoundMuteAction;
	public static Action GameSettingAction;

	[Header("GameManager Settings")]
	public GameSettings gameSettings;
	public bool testMode;
	[HideInInspector]
    public PlayableBlock player;

	[Header("DataTables")]
	public GameDataTable gameDataTable;
	public MapDataTable mapDataTable;

	private void Awake()
	{
		if (player == null)
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayableBlock>();
	}

	private void Start()
    {
		if(!testMode)
		{
			gameInfo = gameSettings.LoadGameInfo();
			GameSettingAction?.Invoke();
		}

        gameState = GameState.GameTitle;

		StartCoroutine(GameLoop());

        FadeScreen.Instance.StartFade(false);

		SoundManager.Instance.PlayMusic("BGM_1", true);
	}

	private void OnApplicationPause(bool pause)
	{
		if(pause)
			SetPause(pause);
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

		GameInitAction?.Invoke();

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
        UIManager.Instance.ShowUIGroup(UIGroupType.Result);

		gameInfo.playerHP = 1;
		gameInfo.startHeight = 0f;

		gameSettings.SaveGameInfo();

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
		return 10 * curTargetHeight.level;
	}

    public bool AddCoin(int addCoin)
    {
		if (gameInfo.coin + addCoin > int.MaxValue)
		{
			Debug.LogError("Coin value max!");
			return false;
		}

		gameInfo.coin += addCoin;

        AddCoinAction?.Invoke();

		Debug.LogFormat("Add coin : {0}", addCoin);
		return true;
    }

	public bool IsUseCoin(int useCoin)
	{
		if (gameInfo.coin < useCoin)
		{
			SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
			return false;
		}
		else
			return true;
	}

	public void UseCoin(int useCoin)
	{
		gameInfo.coin -= useCoin;

		UseCoinAction?.Invoke();

		Debug.LogFormat("Used coin : {0}", useCoin);
	}

	public bool AddGem(int addGem)
	{
		if(gameInfo.gem + addGem > int.MaxValue)
		{
			Debug.LogError("Gem value max!");
			return false;
		}

		gameInfo.gem += addGem;
		AddGemAction?.Invoke();

		SoundManager.Instance.PlaySound2D("Buy_Item");

		Debug.LogFormat("Add gem : {0}", addGem);
		return true;
	}

	public bool IsUseGem(int useGem)
	{
		if (gameInfo.gem < useGem)
		{
			SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
			return false;
		}
		else
			return true;
	}

	public void UseGem(int useGem)
	{
		gameInfo.gem -= useGem;

		UseGemAction?.Invoke();
		SoundManager.Instance.PlaySound2D("Buy_Item");
		Debug.LogFormat("Used gem : {0}", useGem);
	}

	public bool IsAddHP(int addHp)
	{
		if (gameInfo.playerHP + addHp > gameInfo.playerMaxHP)
		{
			SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
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
		if (gameInfo.startHeight + addHeight > maxStartHeight && gameInfo.startHeight + addHeight > gameInfo.lastHeight)
		{
			SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
			return false;
		}
		else
			return true;
	}

	public void AddHeight(float addHeight)
	{
		gameInfo.startHeight += addHeight;

		if (gameInfo.startHeight >= gameInfo.lastHeight || gameInfo.startHeight >= maxStartHeight)
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

    public void SetGameState(GameState state)
    {
        gameState = state;
    }
}
