using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using MysticLights;

[System.Serializable]
public class DeviceSettings
{
	public bool muteBGM;
	public bool muteSE;
	public bool viberate;
	public bool tutorial;
	public PlayableBlockType blockType;
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

	public void SaveInfoToServer()
	{
		if (!GameManager.IsConnected || GameManager.IsPracticeMode)
		{
			isProcessing = false;
			return;
		}

		isProcessing = true;

		GameInfo gameInfo = GameManager.Instance.gameInfo;
		string fileName = string.Format("rpdlafhzjftpdlqmvkdlf{0}", extensionName);
		SaveFileManager.Save<GameInfo>(gameInfo, fileName);
		
		string stringData = JsonUtility.ToJson(gameInfo);
		//GooglePlayManager.Instance.SaveToCloud(stringData);
		FirebaseDBManager.Instance.SendFirebaseDB("gamedata", "", stringData);
		//StartCoroutine(SaveToCloudSync());
		isProcessing = false;

		Debug.LogFormat("Save GameInfo to server : {0}, {1}, {2}, {3}", gameInfo.coin, gameInfo.lastHeight, gameInfo.playerHP, gameInfo.startHeight);
	}

	//IEnumerator SaveToCloudSync()
	//{
	//	Debug.Log("Save Processing start");
	//	UIManager.Instance.ShowPopup(PopupUIType.Waiting, true);

	//	while (GooglePlayManager.Instance.isProcessing)
	//	{
	//		yield return null;
	//	}

	//	UIManager.Instance.ShowPopup(PopupUIType.Waiting, false);

	//	isProcessing = false;

	//	Debug.Log("Save Processing done");
	//}

	public void LoadGameInfo()
	{
		LoadDeviceOptions();
		StartCoroutine(LoadInfoForServer());

		Debug.Log("Load GameSetting");
	}

	void LoadDeviceOptions()
	{
		string fileName = string.Format("{0}{1}", deviceSettingFileName, extensionName);
		DeviceSettings deviceSettings = SaveFileManager.Load<DeviceSettings>(fileName);
		if(deviceSettings == null)
		{
			deviceSettings = new DeviceSettings();
			deviceSettings.muteBGM = false;
			deviceSettings.muteSE = false;
			deviceSettings.viberate = true;
			deviceSettings.tutorial = true;
			deviceSettings.blockType = PlayableBlockType.Circle;
		}

		GameManager.Instance.deviceSettings = deviceSettings;
		
		GameManager.Instance.SetSoundMute(SoundType.BGM, deviceSettings.muteBGM);
		
		GameManager.Instance.SetSoundMute(SoundType.SE, deviceSettings.muteSE);
		
		GameManager.Instance.SetViberate(deviceSettings.viberate);

		GameManager.Instance.curBlockType = deviceSettings.blockType;
		GameManager.Instance.SetPlayableBlockType(deviceSettings.blockType);

		Debug.Log("Load Device Options");
	}

	IEnumerator LoadInfoForServer()
	{
		isProcessing = true;
		UIManager.Instance.ShowPopup(PopupUIType.Waiting, true);
		yield return new WaitForSecondsRealtime(0.5f);

		if (GameManager.IsConnected)
		{
			//GooglePlayManager.Instance.LoadFromCloud(LoadCompleteCallback);
			FirebaseDBManager.Instance.ReceiveFirebaseDB("gamedata", "", LoadCompleteCallback);
		}
		else
		{
			LoadInfoForLocal();

			isProcessing = false;

			GameSettingAction?.Invoke();
		}
	}

	void LoadInfoForLocal()
	{
		string fileName = string.Format("rpdlafhzjftpdlqmvkdlf{0}", extensionName);
		GameInfo gameInfo = SaveFileManager.Load<GameInfo>(fileName);
		if(gameInfo == null)
		{
			gameInfo = new GameInfo();
		}

		CheckDefaultGameInfo(gameInfo);

		GameManager.Instance.gameInfo = gameInfo;

		Debug.LogWarningFormat("Load GameInfo for local : {0}, {1}, {2}, {3}", gameInfo.coin, gameInfo.lastHeight, gameInfo.playerHP, gameInfo.startHeight);

		UIManager.Instance.ShowPopup(PopupUIType.Waiting, false);
	}

	void LoadCompleteCallback(object loadData)
	{
		Debug.Log("Load Data Callback");

		if (loadData != null)
		{
			GameInfo gameInfo = GameManager.Instance.gameInfo;
			Dictionary<string, object> dataDic = loadData as Dictionary<string, object>;

			gameInfo.coin = int.Parse(dataDic["coin"].ToString());
			gameInfo.lastHeight = float.Parse(dataDic["lastHeight"].ToString());
			gameInfo.playerHP = int.Parse(dataDic["playerHP"].ToString());
			gameInfo.playerMaxHP = int.Parse(dataDic["playerMaxHP"].ToString());
			gameInfo.startHeight = float.Parse(dataDic["startHeight"].ToString());

			CheckDefaultGameInfo(gameInfo);

			Debug.LogFormat("Load GameInfo for server : {0}, {1}, {2}, {3}", gameInfo.coin, gameInfo.lastHeight, gameInfo.playerHP, gameInfo.startHeight);

		}

		UIManager.Instance.ShowPopup(PopupUIType.Waiting, false);

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
