using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using MLFramework;

[System.Serializable]
public class DeviceSettings
{
	public bool useBGM;
	public bool useSE;
	public bool viberate;
	public bool tutorial;
	public PlayableBlockType blockType;
	public int bgIndex;
}

public enum SoundType { BGM, SE }
public class GameSettings : MonoBehaviour
{
	private const string deviceSettingFileName = "rpdlatjfwjdvkdlf";
	private const string gameDataFileName = "gd";
	public const string extensionName = ".dla";
	public static Action GameSettingAction;

	[HideInInspector]
	public bool isProcessing;

	private void Awake()
	{
		SoundManager.Instance.bgmMaxVolume = 0.2f;
		SoundManager.Instance.crossFadeTime = 2f;
	}

	public void SaveDeviceOptions()
	{
		DeviceSettings deviceSettings = GameManager.Instance.deviceSettings;
		deviceSettings.blockType = GameManager.Instance.curBlockType;

		string fileName = string.Format("{0}{1}", deviceSettingFileName, extensionName);
		SaveFileManager.Save<DeviceSettings>(deviceSettings, fileName);

		Debug.Log("Save Device Settings");
	}

	public void SaveGameData(bool toCloud = false)
	{
		GameInfo gameInfo = GameManager.Instance.gameInfo;

		isProcessing = true;

		if (toCloud)
		{
			string stringData = JsonUtility.ToJson(gameInfo);
			GooglePlayManager.Instance.SaveToCloud(stringData);
			StartCoroutine(SaveToCloudSync());
		}
		else
		{
			if(GooglePlayManager.IsAuthenticated)
			{
				string fileName = string.Format("{0}{1}", Social.localUser.id, extensionName);
				SaveFileManager.Save<GameInfo>(gameInfo, fileName);
			}

			isProcessing = false;
		}

		//FirebaseDBManager.Instance.SendFirebaseDB("gamedata", "", stringData);
		//isProcessing = false;

		Debug.LogFormat("Save GameInfo : {0}, {1}, {2}, {3}", gameInfo.coin, gameInfo.lastHeight, gameInfo.playerHP, gameInfo.startHeight);
	}

	IEnumerator SaveToCloudSync()
	{
		Debug.Log("Save Processing start");
		UIManager.Instance.ShowLoadingUI(true);

		while (GooglePlayManager.Instance.isProcessing)
		{
			yield return null;
		}

		UIManager.Instance.ShowLoadingUI(false);

		isProcessing = false;

		Debug.Log("Save Processing done");
	}

	public void LoadGameData(bool toCloud = false)
	{
		isProcessing = true;

		if (toCloud)
		{
			UIManager.Instance.ShowLoadingUI(true);

			GooglePlayManager.Instance.LoadFromCloud(LoadCompleteCallback);
		}
		else
		{
			LoadInfoForLocal();
		}

		Debug.Log("Load GameSetting");
	}

	public void LoadDeviceOptions()
	{
		string fileName = string.Format("{0}{1}", deviceSettingFileName, extensionName);

		DeviceSettings deviceSettings = SaveFileManager.Load<DeviceSettings>(fileName);
		if(deviceSettings == null)
		{
			deviceSettings = new DeviceSettings();
			deviceSettings.useBGM = true;
			deviceSettings.useSE = true;
			deviceSettings.viberate = true;
			deviceSettings.tutorial = true;
			deviceSettings.blockType = PlayableBlockType.Circle;
		}

		GameManager gameManager = GameManager.Instance;
		gameManager.deviceSettings = deviceSettings;
		gameManager.SetSoundSetting(SoundType.BGM, deviceSettings.useBGM);
		gameManager.SetSoundSetting(SoundType.SE, deviceSettings.useSE);
		gameManager.curBlockType = deviceSettings.blockType;
		gameManager.SetPlayableBlockType(deviceSettings.blockType);
		gameManager.SetBGColor(deviceSettings.bgIndex);

		Debug.Log("Load Device Options");
	}

	void LoadInfoForLocal()
	{
		GameInfo gameInfo = null;

		if (GooglePlayManager.IsAuthenticated)
		{
			string fileName = string.Format("{0}{1}", Social.localUser.id, extensionName);
			gameInfo = SaveFileManager.Load<GameInfo>(fileName);
		}

		if (gameInfo == null)
			gameInfo = new GameInfo();

		CheckDefaultGameInfo(gameInfo);

		GameManager.Instance.gameInfo = gameInfo;

		Debug.LogFormat("Load GameInfo for local : {0}, {1}, {2}, {3}", gameInfo.coin, gameInfo.lastHeight, gameInfo.playerHP, gameInfo.startHeight);

		UIManager.Instance.ShowLoadingUI(false);

		isProcessing = false;

		LoadDeviceOptions();

		GameSettingAction?.Invoke();
	}

	void LoadCompleteCallback(string loadData)
	{
		Debug.Log("Load Data Callback");

		if (!string.IsNullOrEmpty(loadData))
		{
			GameInfo gameInfo = null;
			gameInfo = JsonUtility.FromJson<GameInfo>(loadData);

			if (gameInfo == null)
				gameInfo = new GameInfo();

			CheckDefaultGameInfo(gameInfo);

			Debug.LogFormat("Load GameInfo for server : {0}, {1}, {2}, {3}", gameInfo.coin, gameInfo.lastHeight, gameInfo.playerHP, gameInfo.startHeight);

			GameManager.Instance.gameInfo = gameInfo;
		}

		UIManager.Instance.ShowLoadingUI(false);

		LoadDeviceOptions();
		SaveGameData();

		isProcessing = false;

		GameSettingAction?.Invoke();
	}

	//void LoadCompleteFirebaseCallback(object loadData)
	//{
	//	Debug.Log("Load Data Callback");

	//	if (loadData != null)
	//	{
	//		GameInfo gameInfo = GameManager.Instance.gameInfo;
	//		Dictionary<string, object> dataDic = loadData as Dictionary<string, object>;

	//		gameInfo.coin = int.Parse(dataDic["coin"].ToString());
	//		gameInfo.lastHeight = float.Parse(dataDic["lastHeight"].ToString());
	//		gameInfo.playerHP = int.Parse(dataDic["playerHP"].ToString());
	//		gameInfo.playerMaxHP = int.Parse(dataDic["playerMaxHP"].ToString());
	//		gameInfo.startHeight = float.Parse(dataDic["startHeight"].ToString());

	//		CheckDefaultGameInfo(gameInfo);

	//		Debug.LogFormat("Load GameInfo for server : {0}, {1}, {2}, {3}", gameInfo.coin, gameInfo.lastHeight, gameInfo.playerHP, gameInfo.startHeight);

	//	}

	//	UIManager.Instance.ShowPopup(PopupUIType.Waiting, false);

	//	isProcessing = false;
	//	GameSettingAction?.Invoke();
	//}

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
}
