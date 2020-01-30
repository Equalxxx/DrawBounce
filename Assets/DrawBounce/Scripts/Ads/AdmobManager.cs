using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using MysticLights;

public enum AdmobAdType { Interstitial, RewardVideo, Banner }
public class AdmobManager : Singleton<AdmobManager>
{
	private AdmobInterstitialAd interstitialAd;
	private AdmobRewardAd rewardAd;
	private AdmobBannerAd bannerAd;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		interstitialAd = GetComponent<AdmobInterstitialAd>();
		rewardAd = GetComponent<AdmobRewardAd>();
		bannerAd = GetComponent<AdmobBannerAd>();
	}

	public void ShowAd(AdmobAdType adType)
	{
		switch(adType)
		{
			case AdmobAdType.Interstitial:
				interstitialAd.ShowAd();
				break;
			case AdmobAdType.RewardVideo:
				rewardAd.ShowAd();
				break;
			case AdmobAdType.Banner:
				bannerAd.ShowAd();
				break;
		}
	}

	public void HideBannerAd()
	{
		bannerAd.HideAd();
	}

	public float GetBannerHeight()
	{
		return bannerAd.GetHeight();
	}

	public float GetBannerWidth()
	{
		return bannerAd.GetWidth();
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
