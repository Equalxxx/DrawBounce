using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobRewardAd : MonoBehaviour
{
	private readonly string unitId = "ca-app-pub-2141587155447938/6851523586";

	private readonly string test_unitId = "ca-app-pub-3940256099942544/5224354917";
	private readonly string test_deviceId = "";

	private RewardedAd rewardedAd;

	public void Start()
	{
		CreateAndLoadRewardedAd();
	}

	public void CreateAndLoadRewardedAd()
	{
		string adUnitId = Debug.isDebugBuild ? test_unitId : unitId;

		rewardedAd = new RewardedAd(adUnitId);

		rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
		rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
		rewardedAd.OnAdClosed += HandleRewardedAdClosed;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the rewarded ad with the request.
		rewardedAd.LoadAd(request);
	}

	public void HandleRewardedAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdLoaded event received");
	}

	public void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleRewardedAdClosed event received");

		CreateAndLoadRewardedAd();
	}

	public void HandleUserEarnedReward(object sender, Reward args)
	{
		string type = args.Type;
		double amount = args.Amount;
		MonoBehaviour.print(
			"HandleRewardedAdRewarded event received for "
						+ amount.ToString() + " " + type);

		AdmobManager.AdRewardVideoAction?.Invoke();
	}

	public void ShowAd()
	{
		if (this.rewardedAd.IsLoaded())
		{
			this.rewardedAd.Show();
		}
	}
}
