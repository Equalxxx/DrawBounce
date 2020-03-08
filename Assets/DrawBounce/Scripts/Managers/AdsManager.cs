using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using MLFramework;

public class AdsManager : Singleton<AdsManager>
{
	public static bool IsRewarded;
	public static bool IsReady
	{
		get
		{
			return Advertisement.IsReady();
		}
	}

	private void HandleShowResult(ShowResult result)
	{
		switch (result)
		{
			case ShowResult.Finished:
				{
					Debug.Log("The ad was successfully shown.");
					IsRewarded = true;
				}
				break;
			case ShowResult.Skipped:
				Debug.Log("The ad was skipped before reaching the end.");
				break;
			case ShowResult.Failed:
				Debug.LogError("The ad failed to be shown.");
				break;
		}
	}

	public void ShowRewarded()
	{
		IsRewarded = false;

		if (Advertisement.IsReady())
		{
			var options = new ShowOptions { resultCallback = HandleShowResult };
			Advertisement.Show("rewardedVideo", options);
		}
		else
		{
			IsRewarded = true;
			Debug.Log("AD FAIL");
		}
	}
}
