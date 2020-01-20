using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public enum AdmobAdType { Interstitial, RewardVideo }
public class AdmobManager : Singleton<AdmobManager>
{
	//private AdmobScreenAd screenAd;
	private AdmobRewardAd rewardAd;

	//public static Action AdInterstitialAction;
	public static Action AdRewardVideoAction;

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
			return;
		}

		DontDestroyOnLoad(gameObject);

		Init();
	}

	private void Init()
	{
		//screenAd = GetComponent<AdmobScreenAd>();
		rewardAd = GetComponent<AdmobRewardAd>();
	}

	public void ShowAd(AdmobAdType adType)
	{
		switch(adType)
		{
			case AdmobAdType.Interstitial:
				//screenAd.ShowAd();
				break;
			case AdmobAdType.RewardVideo:
				rewardAd.ShowAd();
				break;
		}
	}
}
