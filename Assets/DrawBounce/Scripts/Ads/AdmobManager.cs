using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public enum AdmobAdType { Interstitial, RewardVideo }
public class AdmobManager : Singleton<AdmobManager>
{
	private AdmobScreenAd screenAd;
	private AdmobRewardAd rewardAd;

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
		screenAd = GetComponent<AdmobScreenAd>();
		rewardAd = GetComponent<AdmobRewardAd>();
	}

	public void ShowAd(AdmobAdType adType)
	{
		switch(adType)
		{
			case AdmobAdType.Interstitial:
				screenAd.Show();
				break;
			case AdmobAdType.RewardVideo:
				rewardAd.Show();
				break;
		}
	}
}
