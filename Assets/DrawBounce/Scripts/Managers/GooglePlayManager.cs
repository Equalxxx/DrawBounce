using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using MysticLights;

public class GooglePlayManager : Singleton<GooglePlayManager>
{
	private const string saveFile = "drawbouncegamedata";

	public static Action<bool> SignInAction;
	public static Action<bool> OnSavedCloudAction;
	public static Action<bool> OnLoadedCloudAction;

	public static bool IsConnected => Social.localUser.authenticated;

	private void Awake()
	{
		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			.EnableSavedGames()
			.Build();
		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();
	}

	private void Start()
	{
		SignIn();
	}

	public void SignIn()
	{
		if (Social.localUser.authenticated)
			return;

		Social.localUser.Authenticate((bool bSuccess) =>
		{
			if (bSuccess)
			{
				Debug.Log("SignIn Success : " + Social.localUser.userName);
			}
			else
			{
				Debug.LogWarningFormat("SignIn Failed");
			}

			SignInAction?.Invoke(bSuccess);
		});
	}

	public void SignOut()
	{
#if UNITY_ANDROID
		PlayGamesPlatform.Instance.SignOut();
#endif
	}

	public void UnlockAchievement(string achievementId)
	{
#if UNITY_ANDROID
		PlayGamesPlatform.Instance.ReportProgress(achievementId, 100f, null);
#elif UNITY_IOS
		Social.ReportProgress(achievementId, 100f, null);
#endif
	}

	public void ShowAchievementUI()
	{
		if(Social.localUser.authenticated)
		{
			Debug.Log("Show Achievement success");
			Social.ShowAchievementsUI();
		}
		else
		{
			Debug.LogWarning("Show Achievement failed");
		}
	}

	public void ReportScore(int score)
	{
#if UNITY_ANDROID
		PlayGamesPlatform.Instance.ReportScore(score, GPGSIds.leaderboard, (bool success) =>
		{
			if(success)
			{
				Debug.LogFormat("Report score success : {0}", score);
			}
			else
			{
				Debug.LogWarningFormat("Report score failed : {0}", score);
			}
		});
#elif UNITY_IOS
		Social.ReportScore(score, "Ranking", (bool success) =>
		{
			if(success)
			{
				Debug.LogFormat("Report score success : {0}", score);
			}
			else
			{
				Debug.LogWarningFormat("Report score failed : {0}", score);
			}
		});
#endif
	}

	public void ShowLeaderboardUI()
	{
		if (Social.localUser.authenticated)
		{
			Debug.Log("Show Leaderboard success");
			Social.ShowLeaderboardUI();
		}
		else
		{
			Debug.LogWarning("Show Leaderboard failed");
		}
	}

	[HideInInspector]
	public byte[] savedBytes;

	public void SaveToCloud(byte[] bytes)
	{
		savedBytes = bytes;
		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

		if (Social.localUser.authenticated)
		{
			savedGameClient.OpenWithAutomaticConflictResolution(saveFile, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedToSave);
		}
		else
		{
			Debug.LogWarning("Show save to cloud failed");

			OnSavedCloudAction?.Invoke(false);
		}
	}

	void OnSavedGameOpenedToSave(SavedGameRequestStatus status, ISavedGameMetadata game)
	{
		if(status == SavedGameRequestStatus.Success)
		{
			SaveGame(game, savedBytes, DateTime.Now.TimeOfDay);
		}
		else
		{
			Debug.LogWarning("Load saved gamedata failed");

			OnSavedCloudAction?.Invoke(false);
		}
	}

	void SaveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
	{
		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
		SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
		builder = builder.WithUpdatedPlayedTime(totalPlaytime).WithUpdatedDescription(string.Format("Saved game at {0}", DateTime.Now));

		SavedGameMetadataUpdate updatedMetadata = builder.Build();
		savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
	}

	void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
	{
		if(status == SavedGameRequestStatus.Success)
		{
			Debug.Log("Game data saved success");

			OnSavedCloudAction?.Invoke(true);
		}
		else
		{
			Debug.LogWarning("Game data saved failed");

			OnSavedCloudAction?.Invoke(false);
		}
	}

	public void LoadFromCloud()
	{
		if (Social.localUser.authenticated)
		{
			ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

			savedGameClient.OpenWithAutomaticConflictResolution(saveFile, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedToRead);
		}
		else
		{
			Debug.LogWarning("Show load from cloud failed");

			OnLoadedCloudAction?.Invoke(false);
		}
	}

	void OnSavedGameOpenedToRead(SavedGameRequestStatus status, ISavedGameMetadata game)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			LoadGameData(game);
		}
		else
		{
			Debug.LogWarning("Load saved gamedata failed");

			OnLoadedCloudAction?.Invoke(false);
		}
	}

	void LoadGameData(ISavedGameMetadata game)
	{
		ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
		savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
	}

	void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
	{
		if(status == SavedGameRequestStatus.Success)
		{
			Debug.Log("Read gamedata success");

			savedBytes = data;

			OnLoadedCloudAction?.Invoke(true);
		}
		else
		{
			Debug.LogWarning("Read gamedata failed");

			OnLoadedCloudAction?.Invoke(false);
		}
	}
}
