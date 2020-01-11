using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public enum GameState { GameTitle, GamePlay, GameOver }

public class GameManager : Singleton<GameManager>
{
    public GameState gameState;

    public int score;
    public float meter;

	[Header("GameLevel Info")]
	public int level;
	public int levelMeterCost = 100;

    public static Action AddScoreAction;

    public static Action GameInitAction;
    public static Action GamePlayAction;
	public static Action GameOverAction;

    [HideInInspector]
    public Player player;

    public Transform spawnTrans;

	public ScrollBackground scrollBG;

	public bool testMode;

	private void Awake()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
	}

	private void Start()
    {
        gameState = GameState.GameTitle;

		StartCoroutine(GameLoop());

        FadeScreen.Instance.StartFade(false);
    }

    IEnumerator GameLoop()
    {
        while(true)
        {
            yield return StartCoroutine(gameState.ToString());
        }
    }

    IEnumerator GameTitle()
    {
        Debug.Log("Title Menu!");
        UIManager.Instance.ShowUIGroup(UIGroupType.Title);
		level = 1;
		player.InitPlayer();
		GameInitAction?.Invoke();

		while (gameState == GameState.GameTitle)
        {
            yield return null;
        }
    }

    IEnumerator GamePlay()
    {
		Debug.Log("Game Start!");
        UIManager.Instance.ShowUIGroup(UIGroupType.Game);

        GamePlayAction?.Invoke();

		int nextLevel = 0;

		while (gameState == GameState.GamePlay)
        {
			nextLevel = (int)player.height / levelMeterCost + 1;
			if (nextLevel > level)
				level = nextLevel;

            yield return null;
        }
    }

    IEnumerator GameOver()
    {
        Debug.Log("Game Over!");
        UIManager.Instance.ShowUIGroup(UIGroupType.Result);

        GameOverAction?.Invoke();

        while (gameState == GameState.GameOver)
        {
            yield return null;
        }
    }

    public void AddScore(int addScore)
    {
        score += addScore;

        AddScoreAction?.Invoke();
    }

    public void SetGameState(GameState state)
    {
        gameState = state;
    }
}
