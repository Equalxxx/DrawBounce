using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using MysticLights;

public class GooglePlayManager : Singleton<GooglePlayManager>
{
	private const string saveFile = "drawbouncegamedata";
	private ISavedGameMetadata myGame;
	public GameInfo savedGameInfo;

	public static Action<bool> SignInAction;
	public static Action<bool> OnSavedCloudAction;
	public static Action<bool> OnLoadedCloudAction;

	public static bool IsConnected => Social.localUser.authenticated;

	private void Awake()
	{
		if(Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		DontDestroyOnLoad(this.gameObject);

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

	#region Sign

	public void SignIn()
	{
		if (Social.localUser.authenticated)
			return;

		Social.localUser.Authenticate((bool bSuccess) =>
		{
			if (bSuccess)
			{
				Debug.Log("SignIn Success : " + Social.localUser.userName);
				LoadFromCloud();
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

	#endregion

	#region Achievement

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

	#endregion

	#region SendScore

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

	#endregion

	#region Leaderboard

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

	#endregion

	#region Save

	public void SaveToCloud(GameInfo gameData)
	{
		string jsonData = JsonUtility.ToJson(gameData);
		byte[] bytes = Encoding.UTF8.GetBytes(jsonData);

		SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder().Build();
		((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(myGame, update, bytes, SaveCallBack);
	}

	private void SaveCallBack(SavedGameRequestStatus status, ISavedGameMetadata game)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			Debug.Log("Save game data success");
			LoadFromCloud();
			OnSavedCloudAction?.Invoke(true);
		}
		else
		{
			Debug.LogWarning("Save game data failed");
			OnSavedCloudAction?.Invoke(false);
		}
	}

	#endregion

	#region Load

	public void LoadFromCloud()
	{
		((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveFile, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, LoadGame);
	}

	void LoadGame(SavedGameRequestStatus status, ISavedGameMetadata game)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			myGame = game;
			LoadData(myGame);
		}
		else
		{
			Debug.LogWarning("Load game data failed");
			OnLoadedCloudAction?.Invoke(false);
		}
	}

	void LoadData(ISavedGameMetadata game)
	{
		((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(game, LoadDataCallBack);
	}

	void LoadDataCallBack(SavedGameRequestStatus status, byte[] LoadedData)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			try
			{
				string jsonData = Encoding.UTF8.GetString(LoadedData);
				savedGameInfo = JsonUtility.FromJson<GameInfo>(jsonData);
				OnLoadedCloudAction?.Invoke(true);
			}
			catch (Exception e)
			{
				Debug.LogWarningFormat("Load failed : {0}", e);
				OnLoadedCloudAction?.Invoke(false);
			}
		}
		else
		{
			OnLoadedCloudAction?.Invoke(false);
		}
	}

	#endregion
}
