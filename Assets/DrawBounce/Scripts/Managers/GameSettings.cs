﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using MysticLights;

public enum SoundType { BGM, SE }
public class GameSettings : MonoBehaviour
{
	public static Action GameSettingAction;
	public bool isProcessing;

	private void Awake()
	{
		SoundManager.Instance.bgmMaxVolume = 0.2f;
		SoundManager.Instance.crossFadeTime = 2f;
	}

	public void SaveDeviceOptions()
	{
		PlayerPrefs.SetString("MuteBGM", GameManager.Instance.isMuteBGM.ToString());
		PlayerPrefs.SetString("MuteSE", GameManager.Instance.isMuteSE.ToString());
		PlayerPrefs.SetString("Viberate", GameManager.Instance.isVibe.ToString());
		PlayerPrefs.SetString("Tutorial", GameManager.Instance.isTutorial.ToString());
		PlayerPrefs.SetString("BlockType", GameManager.Instance.curBlockType.ToString());

		Debug.Log("Save Device Settings");
	}

	public void SaveInfoToServer()
	{
		if (!GameManager.IsConnected && GameManager.IsPracticeMode)
		{
			isProcessing = false;
			return;
		}

		isProcessing = true;

		GameInfo gameInfo = GameManager.Instance.gameInfo;

		PlayerPrefs.SetInt("Coin", gameInfo.coin);
		PlayerPrefs.SetInt("PlayerHP", gameInfo.playerHP);
		PlayerPrefs.SetInt("PlayerMaxHP", gameInfo.playerMaxHP);
		PlayerPrefs.SetFloat("StartHeight", gameInfo.startHeight);
		PlayerPrefs.SetFloat("LastHeight", gameInfo.lastHeight);

		string stringData = JsonUtility.ToJson(gameInfo);
		GooglePlayManager.Instance.SaveToCloud(stringData);
		StartCoroutine(SaveToCloudSync());
		Debug.LogFormat("Save GameInfo to server : {0}, {1}, {2}, {3}", gameInfo.coin, gameInfo.lastHeight, gameInfo.playerHP, gameInfo.startHeight);
	}

	IEnumerator SaveToCloudSync()
	{
		Debug.Log("Save Processing start");
		UIManager.Instance.ShowWaitingPopupUI(true);

		while (GooglePlayManager.Instance.isProcessing)
		{
			yield return null;
		}

		UIManager.Instance.ShowWaitingPopupUI(false);

		isProcessing = false;

		Debug.Log("Save Processing done");
	}

	public void LoadGameInfo()
	{
		LoadDeviceOptions();
		StartCoroutine(LoadInfoForServer());

		Debug.Log("Load GameSetting");
	}

	void LoadDeviceOptions()
	{
		string strMuteBGM = PlayerPrefs.GetString("MuteBGM", "False");
		bool muteBGM = bool.Parse(strMuteBGM);

		GameManager.Instance.SetSoundMute(SoundType.BGM, muteBGM);

		string strMuteSE = PlayerPrefs.GetString("MuteSE", "False");
		bool muteSE = bool.Parse(strMuteSE);

		GameManager.Instance.SetSoundMute(SoundType.SE, muteSE);

		string strVibe = PlayerPrefs.GetString("Viberate", "True");
		bool vibe = bool.Parse(strVibe);

		GameManager.Instance.SetViberate(vibe);

		string strTuto = PlayerPrefs.GetString("Tutorial", "True");
		bool tuto = bool.Parse(strTuto);

		GameManager.Instance.isTutorial = tuto;

		if (GameManager.Instance.isTutorial)
		{
			UIManager.Instance.tutorialUI.Show(true);
			GameManager.Instance.isTutorial = false;
			SaveDeviceOptions();
		}

		string strBlockType = PlayerPrefs.GetString("BlockType", "Circle");
		PlayableBlockType blockType = ParseEnum<PlayableBlockType>(strBlockType, PlayableBlockType.Circle);

		GameManager.Instance.SetPlayableBlockType(blockType);

		Debug.Log("Load Device Options");
	}

	IEnumerator LoadInfoForServer()
	{
		isProcessing = true;
		UIManager.Instance.ShowWaitingPopupUI(true);
		yield return new WaitForSecondsRealtime(0.5f);

		if (GameManager.IsConnected)
		{
			GooglePlayManager.Instance.LoadFromCloud(LoadCompleteCallback);
		}
		else
		{
			LoadInfoForLocal();

			isProcessing = false;
			UIManager.Instance.ShowWaitingPopupUI(false);
			GameSettingAction?.Invoke();
		}
	}

	void LoadInfoForLocal()
	{
		GameInfo gameInfo = GameManager.Instance.gameInfo;

		gameInfo.coin = PlayerPrefs.GetInt("Coin", 0);
		gameInfo.playerHP = PlayerPrefs.GetInt("PlayerHP", 1);
		gameInfo.playerMaxHP = PlayerPrefs.GetInt("PlayerMaxHP", 5);
		gameInfo.startHeight = PlayerPrefs.GetFloat("StartHeight", 0f);
		gameInfo.lastHeight = PlayerPrefs.GetFloat("LastHeight", 0f);

		Debug.LogWarningFormat("Load GameInfo for local : {0}, {1}, {2}, {3}", gameInfo.coin, gameInfo.lastHeight, gameInfo.playerHP, gameInfo.startHeight);

		UIManager.Instance.ShowWaitingPopupUI(false);
	}

	void LoadCompleteCallback(string loadData)
	{
		if(loadData != "")
		{
			GameInfo gameInfo = JsonUtility.FromJson<GameInfo>(loadData);
			CheckDefaultGameInfo(gameInfo);

			GameManager.Instance.gameInfo = gameInfo;

			Debug.LogFormat("Load GameInfo for server : {0}, {1}, {2}, {3}", gameInfo.coin, gameInfo.lastHeight, gameInfo.playerHP, gameInfo.startHeight);

			UIManager.Instance.ShowWaitingPopupUI(false);
		}
		else
		{
			LoadInfoForLocal();
		}

		isProcessing = false;
		GameSettingAction?.Invoke();
	}

	void CheckDefaultGameInfo(GameInfo gameInfo)
	{
		if (gameInfo.coin < 0)
			gameInfo.coin = 0;

		if (gameInfo.playerHP < 1)
			gameInfo.playerHP = 1;

		if (gameInfo.playerMaxHP < 5)
			gameInfo.playerMaxHP = 5;

		if (gameInfo.startHeight < 0f)
			gameInfo.startHeight = 0f;

		if (gameInfo.lastHeight < 0f)
			gameInfo.lastHeight = 0f;
	}

	T ParseEnum<T>(string value, T defaultValue) where T : struct
	{
		try
		{
			T enumValue;
			if (!Enum.TryParse(value, true, out enumValue))
			{
				return defaultValue;
			}
			return enumValue;
		}
		catch (Exception)
		{
			return defaultValue;
		}
	}
}
