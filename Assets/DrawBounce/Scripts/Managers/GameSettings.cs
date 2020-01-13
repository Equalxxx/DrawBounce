using System.Collections;
using System.Collections.Generic;
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

	public GameInfo LoadGameInfo()
	{
		if (GameManager.Instance.testMode)
			return null;

		GameInfo gameInfo = new GameInfo();

		LoadLocalInfo(gameInfo);
		LoadServerInfo(gameInfo);

		Debug.Log("Load GameSetting");

		return gameInfo;
	}

	void LoadLocalInfo(GameInfo gameInfo)
	{
		// Game Info
		gameInfo.coin = PlayerPrefs.GetInt("Coin", 0);
		if (gameInfo.coin < 0)
			gameInfo.coin = 0;

		gameInfo.playerHP = PlayerPrefs.GetInt("PlayerHP", 1);
		if (gameInfo.playerHP < 1)
			gameInfo.playerHP = 1;

		gameInfo.playerMaxHP = PlayerPrefs.GetInt("PlayerMaxHP", 5);
		if (gameInfo.playerMaxHP < 5)
			gameInfo.playerMaxHP = 5;

		gameInfo.startHeight = PlayerPrefs.GetFloat("StartHeight", 0f);
		if (gameInfo.startHeight < 0f)
			gameInfo.startHeight = 0f;

		gameInfo.lastHeight = PlayerPrefs.GetFloat("LastHeight", 0f);
		if (gameInfo.lastHeight < 0f)
			gameInfo.lastHeight = 0f;

		// Sound Settings
		string strMuteBGM = PlayerPrefs.GetString("MuteBGM", "False");
		bool muteBGM = bool.Parse(strMuteBGM);

		GameManager.Instance.SetSoundMute(SoundType.BGM, muteBGM);

		string strMuteSE = PlayerPrefs.GetString("MuteSE", "False");
		bool muteSE = bool.Parse(strMuteSE);

		GameManager.Instance.SetSoundMute(SoundType.SE, muteSE);
	}

	void LoadServerInfo(GameInfo gameInfo)
	{
		gameInfo.gem = PlayerPrefs.GetInt("Gem", 0);
		if (gameInfo.gem < 0)
			gameInfo.gem = 0;
	}

	public void SaveGameInfo()
	{
		if (GameManager.Instance.testMode)
			return;

		GameInfo gameInfo = GameManager.Instance.gameInfo;

		SaveLocalInfo(gameInfo);
		SaveServerInfo(gameInfo);

		Debug.Log("Save GameSetting");
	}

	void SaveLocalInfo(GameInfo gameInfo)
	{
		PlayerPrefs.SetInt("Coin", gameInfo.coin);
		PlayerPrefs.SetInt("PlayerHP", gameInfo.playerHP);
		PlayerPrefs.SetInt("PlayerMaxHP", gameInfo.playerMaxHP);
		PlayerPrefs.SetFloat("StartHeight", gameInfo.startHeight);
		PlayerPrefs.SetFloat("LastHeight", gameInfo.lastHeight);

		SaveSoundMute();
	}

	public void SaveSoundMute()
	{
		PlayerPrefs.SetString("MuteBGM", GameManager.Instance.isMuteBGM.ToString());
		PlayerPrefs.SetString("MuteSE", GameManager.Instance.isMuteSE.ToString());
	}

	void SaveServerInfo(GameInfo gameInfo)
	{
		PlayerPrefs.SetInt("Gem", gameInfo.gem);
	}
}
