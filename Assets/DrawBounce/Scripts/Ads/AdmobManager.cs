using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using MLFramework;

public enum AdmobAdType { Interstitial, RewardVideo, Banner }
public class AdmobManager : Singleton<AdmobManager>
{
	//Banner Id
	private readonly string bannerUnitId = "ca-app-pub-2141587155447938/9006675959";
	private readonly string test_bannerUnitId = "ca-app-pub-3940256099942544/6300978111";
	//Interstitial
	private readonly string interstitialUnitId = "ca-app-pub-2141587155447938/6875380797";
	private readonly string test_interstitialUnitId = "ca-app-pub-3940256099942544/8691691433";
	//Reward
	private readonly string rewardUnitId = "ca-app-pub-2141587155447938/6851523586";
	private readonly string test_rewardUnitId = "ca-app-pub-3940256099942544/5224354917";

	private BannerView bannerAd;
	private InterstitialAd interstitialAd;
	private RewardedAd rewardedAd;

	public static bool IsRewarded;
	public static bool IsShowAd;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		bannerAd = CreateAndLoadBannerAd();
		bannerAd.Hide();

		interstitialAd = CreateAndLoadInterstitialAd();

		rewardedAd = CreateAndLoadRewardedAd();
	}

	#region BannerAd

	BannerView CreateAndLoadBannerAd()
	{
		string adUnitId = test_bannerUnitId;

		AdSize adSize = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
		BannerView banner = new BannerView(adUnitId, adSize, AdPosition.Top);
		//BannerView banner = new BannerView(test_unitId, AdSize.SmartBanner, AdPosition.Top);
		
		banner.OnAdLoaded += HandleOnAdLoaded_banner;
		banner.OnAdFailedToLoad += HandleOnAdFailedToLoad_banner;
		banner.OnAdOpening += HandleOnAdOpened_banner;
		banner.OnAdClosed += HandleOnAdClosed_banner;
		banner.OnAdLeavingApplication += HandleOnAdLeavingApplication_banner;

		AdRequest request = new AdRequest.Builder().Build();

		banner.LoadAd(request);

		return banner;
	}

	public void HandleOnAdLoaded_banner(object sender, EventArgs args)
	{
		Debug.Log("HandleAdLoaded event received_banner");
		Debug.LogFormat("Ad Height: {0}, width: {1}",
			this.bannerAd.GetHeightInPixels(),
			this.bannerAd.GetWidthInPixels());
	}

	public void HandleOnAdFailedToLoad_banner(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.Log("HandleFailedToReceiveAd_banner event received with message: "
							+ args.Message);
	}

	public void HandleOnAdOpened_banner(object sender, EventArgs args)
	{
		Debug.Log("HandleAdOpened event received_banner");

	}

	public void HandleOnAdClosed_banner(object sender, EventArgs args)
	{
		Debug.Log("HandleAdClosed event received_banner");

		bannerAd = CreateAndLoadBannerAd();
	}

	public void HandleOnAdLeavingApplication_banner(object sender, EventArgs args)
	{
		Debug.Log("HandleAdLeavingApplication event received_banner");
	}

	#endregion

	#region InterstitialAd

	InterstitialAd CreateAndLoadInterstitialAd()
	{
		string adUnitId = test_interstitialUnitId;

		InterstitialAd ad = new InterstitialAd(adUnitId);
		
		ad.OnAdLoaded += HandleOnAdLoaded;
		ad.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		ad.OnAdOpening += HandleOnAdOpened;
		ad.OnAdClosed += HandleOnAdClosed;
		ad.OnAdLeavingApplication += HandleOnAdLeavingApplication;
		
		AdRequest request = new AdRequest.Builder().Build();
		ad.LoadAd(request);

		return ad;
	}

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		Debug.Log("HandleAdLoaded event received");
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		Debug.Log("HandleFailedToReceiveAd event received with message: "
							+ args.Message);
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		Debug.Log("HandleAdOpened event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		interstitialAd = CreateAndLoadInterstitialAd();
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		Debug.Log("HandleAdLeavingApplication event received");
	}

	#endregion

	#region RewardAD

	RewardedAd CreateAndLoadRewardedAd()
	{
		string adUnitId = test_rewardUnitId;

		RewardedAd ad = new RewardedAd(adUnitId);

		ad.OnAdLoaded += HandleRewardedAdLoaded;
		ad.OnUserEarnedReward += HandleUserEarnedReward;
		ad.OnAdClosed += HandleRewardedAdClosed;
		
		AdRequest request = new AdRequest.Builder().Build();
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

	#endregion

	public void ShowAd(AdmobAdType adType)
	{
		switch (adType)
		{
			case AdmobAdType.Interstitial:
				interstitialAd.Show();
				break;
			case AdmobAdType.RewardVideo:
				rewardedAd.Show();
				break;
			case AdmobAdType.Banner:
				bannerAd.Show();
				break;
		}
	}

	public void HideBannerAd()
	{
		bannerAd.Hide();
	}

	public float GetBannerWidth()
	{
		return bannerAd.GetWidthInPixels();
	}

	public float GetBannerHeight()
	{
		return bannerAd.GetHeightInPixels();
	}

	public int GetAdSizeWidth()
	{
		return AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth).Width;
	}

	public int GetAdSizeHeight()
	{
		return AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth).Height;
	}
}
