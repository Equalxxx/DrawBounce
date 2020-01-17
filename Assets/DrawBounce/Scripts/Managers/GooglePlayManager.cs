using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayManager : MonoBehaviour
{
	bool bWait = false;

	private void Awake()
	{
		PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
		PlayGamesPlatform.DebugLogEnabled = true;
		PlayGamesPlatform.Activate();

		OnLogin();
	}

	public void OnLogin()
	{
		if (!Social.localUser.authenticated)
		{
			Social.localUser.Authenticate((bool bSuccess) =>
			{
				if (bSuccess)
				{
					Debug.Log("Success : " + Social.localUser.userName);
				}
				else
				{
					Debug.Log("Fall");
				}
			});
		}
	}

	public void OnLogOut()
	{
		((PlayGamesPlatform)Social.Active).SignOut();
	}
}
