using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoyconFramework;

public enum GameState { GameTitle, GamePlay, GameOver }

public class GameManager : Singleton<GameManager>
{
    public GameState gameState;

    public int score;
    public float meter;

    public static Action AddScoreAction;
    public static Action GameStartAction;
    public static Action GameOverAction;

    [HideInInspector]
    public Player player;
    public Transform spawnTrans;

    public GenerateMap gameMapManager;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

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
        gameMapManager.RemoveBlocks();

        Debug.Log("Title Menu!");
        UIManager.Instance.ShowUIGroup(UIGroupType.Title);
        player.InitPlayer();

        while (gameState == GameState.GameTitle)
        {
            //if(Input.anyKeyDown)
            //{
            //    gameState = GameState.GamePlay;
            //}

            yield return null;
        }
    }

    IEnumerator GamePlay()
    {
        gameMapManager.Generate();

        Debug.Log("Game Start!");
        UIManager.Instance.ShowUIGroup(UIGroupType.Game);

        while(gameState == GameState.GamePlay)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                gameState = GameState.GameOver;
            }

            yield return null;
        }

        GameStartAction?.Invoke();
    }

    IEnumerator GameOver()
    {
        Debug.Log("Game Over!");
        UIManager.Instance.ShowUIGroup(UIGroupType.Result);

        while (gameState == GameState.GameOver)
        {
            //if (Input.anyKeyDown)
            //{
            //    gameState = GameState.GameTitle;
            //}

            yield return null;
        }

        GameOverAction?.Invoke();
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
