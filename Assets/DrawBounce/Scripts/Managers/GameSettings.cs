using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using MysticLights;

public enum SoundType { BGM, SE }
public class GameSettings : MonoBehaviour
{
	private void Awake()
	{
		SoundManager.Instance.bgmMaxVolume = 0.2f;
		SoundManager.Instance.crossFadeTime = 2f;
	}

	private void OnEnable()
	{
		GooglePlayManager.OnSavedCloudAction += SaveCompleteCallback;
		GooglePlayManager.OnLoadedCloudAction += LoadCompleteCallback;
	}

	private void OnDisable()
	{
		GooglePlayManager.OnSavedCloudAction -= SaveCompleteCallback;
		GooglePlayManager.OnLoadedCloudAction -= LoadCompleteCallback;
	}

	public void SaveGameInfo()
	{
		if (GameManager.Instance.testMode)
			return;
		if (!GameManager.IsInternetConnected)
			return;
		if (!GooglePlayManager.IsAuthenticated)
			return;

		SaveDeviceOptions();
		SaveInfoToServer();

		Debug.Log("Save GameSetting");
	}

	public void SaveDeviceOptions()
	{
		PlayerPrefs.SetString("MuteBGM", GameManager.Instance.isMuteBGM.ToString());
		PlayerPrefs.SetString("MuteSE", GameManager.Instance.isMuteSE.ToString());
		PlayerPrefs.SetString("Viberate", GameManager.Instance.isVibe.ToString());
		PlayerPrefs.SetString("BlockType", GameManager.Instance.curBlockType.ToString());
	}

	void SaveInfoToServer()
	{
		GameInfo gameInfo = GameManager.Instance.gameInfo;
		GooglePlayManager.Instance.SaveToCloud(gameInfo);
	}

	void SaveCompleteCallback(bool success)
	{
		if (success)
		{
			GameInfo gameInfo = GameManager.Instance.gameInfo;

			PlayerPrefs.SetInt("Coin", gameInfo.coin);
			PlayerPrefs.SetInt("PlayerHP", gameInfo.playerHP);
			PlayerPrefs.SetInt("PlayerMaxHP", gameInfo.playerMaxHP);
			PlayerPrefs.SetFloat("StartHeight", gameInfo.startHeight);
			PlayerPrefs.SetFloat("LastHeight", gameInfo.lastHeight);
			Debug.Log("Save game info to server");
		}
		else
		{
			Debug.LogWarning("Save game info to local");
		}
	}

	public void LoadGameInfo()
	{
		if (GameManager.Instance.testMode)
			return;

		LoadDeviceOptions();
		//LoadInfoForServer();

		Debug.Log("Load GameSetting");
	}

	public void LoadDeviceOptions()
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

		string strBlockType = PlayerPrefs.GetString("BlockType", "Circle");
		PlayableBlockType blockType = ParseEnum<PlayableBlockType>(strBlockType, PlayableBlockType.Circle);

		GameManager.Instance.SetPlayableBlockType(blockType);
	}

	//void LoadInfoForServer()
	//{
	//	GooglePlayManager.Instance.LoadFromCloud();
	//}

	void LoadCompleteCallback(bool success)
	{
		if(success)
		{
			GameInfo gameInfo = GameManager.Instance.gameInfo;
			//byte[] bytes = GooglePlayManager.Instance.savedBytes;

			//gameInfo = Deserialize<GameInfo>(bytes);
			gameInfo = GooglePlayManager.Instance.savedGameInfo;

			CheckDefaultGameInfo(gameInfo);

			Debug.Log("Load game info for server");
		}
		else
		{
			GameInfo gameInfo = GameManager.Instance.gameInfo;

			gameInfo.coin = PlayerPrefs.GetInt("Coin", 0);
			gameInfo.playerHP = PlayerPrefs.GetInt("PlayerHP", 1);
			gameInfo.playerMaxHP = PlayerPrefs.GetInt("PlayerMaxHP", 5);
			gameInfo.startHeight = PlayerPrefs.GetFloat("StartHeight", 0f);
			gameInfo.lastHeight = PlayerPrefs.GetFloat("LastHeight", 0f);

			CheckDefaultGameInfo(gameInfo);

			Debug.LogWarning("Load game info for local");
		}
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

	//byte[] ObjectToByteArraySerialize(object obj)
	//{
	//	using (var stream = new MemoryStream())
	//	{
	//		BinaryFormatter bf = new BinaryFormatter();
	//		bf.Serialize(stream, obj);
	//		stream.Flush();
	//		stream.Position = 0;

	//		return stream.ToArray();
	//	}
	//}

	//T Deserialize<T>(byte[] byteData)
	//{
	//	using (var stream = new MemoryStream(byteData))
	//	{
	//		BinaryFormatter bf = new BinaryFormatter();
	//		stream.Seek(0, SeekOrigin.Begin);

	//		return (T)bf.Deserialize(stream);
	//	}
	//}

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
