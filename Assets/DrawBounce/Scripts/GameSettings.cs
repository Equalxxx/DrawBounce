using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public class GameSettings : MonoBehaviour
{
	private float originVolume = 0.8f;

	private void Awake()
	{
		SoundManager.Instance.bgmMaxVolume = 0.2f;
	}

	public GameInfo LoadGameInfo()
	{
		GameInfo gameInfo = new GameInfo();

		LoadLocalInfo();
		LoadServerInfo(gameInfo);

		Debug.Log("Load GameSetting");

		return gameInfo;
	}

	void LoadLocalInfo()
	{
		string getString = PlayerPrefs.GetString("SoundMute", "False");
		bool mute = bool.Parse(getString);

		GameManager.Instance.SetSoundMute(mute);
	}

	void LoadServerInfo(GameInfo gameInfo)
	{
		gameInfo.coin = PlayerPrefs.GetInt("Coin", 0);
		if (gameInfo.coin < 0)
			gameInfo.coin = 0;

		gameInfo.gem = PlayerPrefs.GetInt("Gem", 0);
		if (gameInfo.gem < 0)
			gameInfo.gem = 0;

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
	}

	public void SaveGameInfo()
	{
		GameInfo gameInfo = GameManager.Instance.gameInfo;

		SaveLocalInfo();
		SaveServerInfo(gameInfo);

		Debug.Log("Save GameSetting");
	}

	void SaveLocalInfo()
	{
		PlayerPrefs.SetString("SoundMute", GameManager.Instance.isSoundMute.ToString());
	}

	void SaveServerInfo(GameInfo gameInfo)
	{
		PlayerPrefs.SetInt("Coin", gameInfo.coin);
		PlayerPrefs.SetInt("Gem", gameInfo.gem);
		PlayerPrefs.SetInt("PlayerHP", gameInfo.playerHP);
		PlayerPrefs.SetInt("PlayerMaxHP", gameInfo.playerMaxHP);
		PlayerPrefs.SetFloat("StartHeight", gameInfo.startHeight);
		PlayerPrefs.SetFloat("LastHeight", gameInfo.lastHeight);
	}
}
