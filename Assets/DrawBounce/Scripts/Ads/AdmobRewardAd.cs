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

	public static bool IsRewarded;
	public static bool IsShowAd;

	public void Start()
	{
		rewardedAd = CreateAndLoadRewardedAd();
	}

	RewardedAd CreateAndLoadRewardedAd()
	{
		string adUnitId = test_unitId;

		RewardedAd ad = new RewardedAd(adUnitId);

		ad.OnAdLoaded += HandleRewardedAdLoaded;
		ad.OnUserEarnedReward += HandleUserEarnedReward;
		ad.OnAdClosed += HandleRewardedAdClosed;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the rewarded ad with the request.
		ad.LoadAd(request);

		return ad;
	}

	void HandleRewardedAdLoaded(object sender, EventArgs args)
	{
		Debug.Log("HandleRewardedAdLoaded event received");
	}

	void HandleRewardedAdClosed(object sender, EventArgs args)
	{
		Debug.Log("HandleRewardedAdClosed event received");

		this.rewardedAd = CreateAndLoadRewardedAd();
		IsShowAd = false;
	}

	void HandleUserEarnedReward(object sender, Reward args)
	{
		string type = args.Type;
		double amount = args.Amount;
		Debug.Log("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);

		IsRewarded = true;
	}

	public void ShowAd()
	{
		if (this.rewardedAd.IsLoaded())
		{
			this.rewardedAd.Show();
		}
	}
}
