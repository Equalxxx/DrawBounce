using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public class GameSettings : MonoBehaviour
{
	public bool isSoundMute;
	private float originVolume;

	private void Awake()
	{
		SoundManager.Instance.masterVolume = 0.8f;
		SoundManager.Instance.bgmMaxVolume = 0.2f;

		originVolume = SoundManager.Instance.masterVolume;
	}

	public void SetSoundMute(bool mute)
	{
		isSoundMute = mute;

		if(isSoundMute)
		{
			SoundManager.Instance.masterVolume = 0f;
		}
		else
		{
			SoundManager.Instance.masterVolume = originVolume;
		}
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
		GameManager.Instance.isSoundMute = bool.Parse(PlayerPrefs.GetString("SoundMute", "false"));
	}

	void LoadServerInfo(GameInfo gameInfo)
	{
		gameInfo.score = PlayerPrefs.GetInt("Score", 0);
		if (gameInfo.score < 0)
			gameInfo.score = 0;

		gameInfo.playerHP = PlayerPrefs.GetInt("PlayerHP", 1);
		if (gameInfo.playerHP < 1)
			gameInfo.playerHP = 1;

		gameInfo.playerMaxHP = PlayerPrefs.GetInt("PlayerMaxHP", 5);
		if (gameInfo.playerMaxHP < 5)
			gameInfo.playerMaxHP = 5;

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
		PlayerPrefs.SetInt("Score", gameInfo.score);
		PlayerPrefs.SetInt("PlayerHP", gameInfo.playerHP);
		PlayerPrefs.SetInt("PlayerMaxHP", gameInfo.playerMaxHP);
		PlayerPrefs.SetFloat("LastHeight", gameInfo.lastHeight);
	}
}
