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
using UnityEngine.Networking;

public class GooglePlayManager : Singleton<GooglePlayManager>
{
	private const string saveFile = "drawbouncegamedata";

	public static bool IsSignInProcess = false;
	public static bool IsAuthenticated => Social.localUser.authenticated;

	public static Action<bool> SignInAction;

	private void Awake()
	{
		if (Instance != this)
		{
			Destroy(this.gameObject);
			return;
		}

		DontDestroyOnLoad(this.gameObject);

		PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
			.EnableSavedGames()
			.Build();
		PlayGamesPlatform.InitializeInstance(config);
		PlayGamesPlatform.DebugLogEnabled = false;
		PlayGamesPlatform.Activate();
	}

	#region Sign

	public void SignIn()
	{
		if (Social.localUser.authenticated)
			return;

		IsSignInProcess = false;

		//Social.localUser.Authenticate((bool bSuccess) =>
		PlayGamesPlatform.Instance.Authenticate((bool bSuccess) =>
		{
			if (bSuccess)
			{
				Debug.Log("SignIn Success : " + Social.localUser.userName);
			}
			else
			{
				Debug.LogWarningFormat("SignIn Failed");
			}

			IsSignInProcess = true;
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
		PlayGamesPlatform.Instance.ReportProgress(achievementId, 100f, SendAchievementCallback);
#elif UNITY_IOS
		Social.ReportProgress(achievementId, 100f, null);
#endif
	}

	void SendAchievementCallback(bool success)
	{
		if (success)
			Debug.Log("Send Achievement Success");
		else
			Debug.LogWarning("Send Achievement Failed");
	}

	public void ShowAchievementUI()
	{
		if (Social.localUser.authenticated)
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
			if (success)
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

	#region Save & Load

	public bool isProcessing
	{
		get;
		private set;
	}

	public string loadedData
	{
		get;
		private set;
	}

	private void ProcessCloudData(byte[] cloudData)
	{
		if (cloudData == null)
		{
			Debug.Log("No Data saved to the cloud");
			return;
		}

		string progress = BytesToString(cloudData);
		loadedData = progress;
	}

	public void LoadFromCloud(Action<string> afterLoadAction)
	{
		if (IsAuthenticated && !isProcessing)
		{
			StartCoroutine(LoadFromCloudRoutin(afterLoadAction));
		}
		else
		{
			Debug.LogWarningFormat("LoadFromCloud failed. IsAuthenticated : {0}", IsAuthenticated);
		}
	}

	private IEnumerator LoadFromCloudRoutin(Action<string> loadAction)
	{
		isProcessing = true;
		Debug.Log("Loading game progress from the cloud.");

		((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(
			saveFile,
			DataSource.ReadCacheOrNetwork,
			ConflictResolutionStrategy.UseLongestPlaytime,
			OnFileOpenToLoad);

		while (isProcessing)
		{
			yield return null;
		}

		loadAction.Invoke(loadedData);
	}

	public void SaveToCloud(string dataToSave)
	{
		if (IsAuthenticated)
		{
			loadedData = dataToSave;
			isProcessing = true;
			((PlayGamesPlatform)Social.Active).SavedGame.OpenWithAutomaticConflictResolution(saveFile, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, OnFileOpenToSave);
		}
		else
		{
			Debug.LogWarningFormat("SaveToCloud failed. IsAuthenticated : {0}", IsAuthenticated);
		}
	}

	private void OnFileOpenToSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			byte[] data = StringToBytes(loadedData);

			SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

			SavedGameMetadataUpdate updatedMetadata = builder.Build();

			((PlayGamesPlatform)Social.Active).SavedGame.CommitUpdate(metaData, updatedMetadata, data, OnGameSave);
		}
		else
		{
			Debug.LogWarning("Error opening Saved Game" + status);
		}
	}

	private void OnFileOpenToLoad(SavedGameRequestStatus status, ISavedGameMetadata metaData)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			((PlayGamesPlatform)Social.Active).SavedGame.ReadBinaryData(metaData, OnGameLoad);
		}
		else
		{
			Debug.LogWarning("Error opening Saved Game" + status);
		}
	}

	private void OnGameLoad(SavedGameRequestStatus status, byte[] bytes)
	{
		if (status != SavedGameRequestStatus.Success)
		{
			Debug.LogWarning("Error Saving" + status);
		}
		else
		{
			ProcessCloudData(bytes);
		}

		isProcessing = false;
	}

	private void OnGameSave(SavedGameRequestStatus status, ISavedGameMetadata metaData)
	{
		if (status == SavedGameRequestStatus.Success)
		{
			Debug.Log("Success Save To Cloud");
		}
		else
		{
			Debug.LogWarning("Error Saving" + status);
		}

		isProcessing = false;
	}

	private byte[] StringToBytes(string stringToConvert)
	{
		return Encoding.UTF8.GetBytes(stringToConvert);
	}

	private string BytesToString(byte[] bytes)
	{
		return Encoding.UTF8.GetString(bytes);
	}

	#endregion
}
