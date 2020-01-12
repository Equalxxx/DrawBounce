using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public enum GameState { GameTitle, GamePlay, GameOver, EnterShop }

[System.Serializable]
public class GameInfo
{
	public int score;
	public int playerHP;
	public int playerMaxHP;
	public float startMeter;
	public float lastHeight;
}

public class GameManager : Singleton<GameManager>
{
	[Header("Game Play Info")]
    public GameState gameState;
	public GameInfo gameInfo;

	[Header("GameLevel Info")]
	public int level;
	public int levelMeterCost = 100;
	public float getScoreHeight = 30f;
	public int addScoreValue = 10;

	[Header("Game Settings")]
	public bool isPause;
	public bool isSoundMute;

	// Game State Actions
	public static Action GameInitAction;
    public static Action GamePlayAction;
	public static Action GameOverAction;

	// Game Play Actions
	public static Action AddScoreAction;
	public static Action UseScoreAction;
	public static Action AddPlayerHPAction;
	public static Action<float> SetPlayAction;
	public static Action<bool> PauseAction;
	
	[Header("GameManager Settings")]
	public GameSettings gameSettings;
	public bool testMode;
	[HideInInspector]
    public Player player;

	private void Awake()
	{
		if (player == null)
			player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

		//gameSettings = GetComponent<GameSettings>();
		gameInfo = gameSettings.LoadGameInfo();
	}

	private void Start()
    {
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

		level = 1;
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

		if(gameInfo.startMeter > 0f)
			SetPlayAction?.Invoke(gameInfo.startMeter);

		GamePlayAction?.Invoke();

		int nextLevel = 0;

		while (gameState == GameState.GamePlay)
        {
			nextLevel = (int)player.height / levelMeterCost + 1;
			if (nextLevel > level)
				level = nextLevel;

            yield return null;
        }

		Debug.Log("Game play is done!");
	}

    IEnumerator GameOver()
    {
        Debug.Log("Game over!");
        UIManager.Instance.ShowUIGroup(UIGroupType.Result);

		gameInfo.playerHP = 1;
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

    public void AddScore()
    {
		if (gameInfo.score + addScoreValue > int.MaxValue)
		{
			Debug.LogError("Score integer value max!");
			return;
		}

		gameInfo.score += addScoreValue;

        AddScoreAction?.Invoke();

		Debug.LogFormat("Add score : {0}", addScoreValue);
    }

	public bool IsUseScore(int useScore)
	{
		if (gameInfo.score < useScore)
		{
			SoundManager.Instance.PlaySound2D("Buy_Heart_Notwork");
			return false;
		}
		else
			return true;
	}

	public void UseScore(int useScore)
	{
		gameInfo.score -= useScore;

		UseScoreAction?.Invoke();

		Debug.LogFormat("Used score : {0}", useScore);
	}

	public bool IsAddPlayerHP(int addHp)
	{
		if (gameInfo.playerHP + addHp > gameInfo.playerMaxHP)
		{
			SoundManager.Instance.PlaySound2D("Buy_Heart_Notwork");
			return false;
		}
		else
			return true;
	}

	public void AddPlayerHP(int addHp)
	{
		gameInfo.playerHP += addHp;

		if (gameInfo.playerHP == gameInfo.playerMaxHP)
		{
			SoundManager.Instance.PlaySound2D("Buy_Heart_End");
		}
		else
		{
			SoundManager.Instance.PlaySound2D("Buy_Heart");
		}

		AddPlayerHPAction?.Invoke();

		Debug.LogFormat("Add player hp : {0}", addHp);
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
