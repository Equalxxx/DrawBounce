using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobRewardAd : MonoBehaviour
{
	private readonly string unitId = "ca-app-pub-2141587155447938/6851523586";

	private readonly string test_unitId = "ca-app-pub-3940256099942544/8691691433";
	private readonly string test_deviceId = "";

	private RewardBasedVideoAd rewardAd;

	private void InitAd()
	{
		string id = Debug.isDebugBuild ? test_unitId : unitId;

		MobileAds.Initialize(id);
		rewardAd = RewardBasedVideoAd.Instance;

		AdRequest request = new AdRequest.Builder().AddTestDevice(test_deviceId).Build();

		rewardAd.LoadAd(request, id);

		//광고 요청이 성공적으로 로드되면 호출됩니다.
		rewardAd.OnAdLoaded += OnAdLoaded;
		//광고 요청을 로드하지 못했을 때 호출됩니다.
		rewardAd.OnAdFailedToLoad += OnAdFailedToLoad;
		//광고가 표시될 때 호출됩니다.
		rewardAd.OnAdOpening += OnAdOpening;
		//광고가 재생되기 시작하면 호출됩니다.
		rewardAd.OnAdStarted += OnAdStarted;
		//사용자가 비디오 시청을 통해 보상을 받을 때 호출됩니다.
		rewardAd.OnAdRewarded += OnAdRewarded;
		//광고가 닫힐 때 호출됩니다.
		rewardAd.OnAdClosed += OnAdClosed;
		//광고 클릭으로 인해 사용자가 애플리케이션을 종료한 경우 호출됩니다.
		rewardAd.OnAdLeavingApplication += OnAdLeavingApplication;
	}

	void OnAdLoaded(object sender, EventArgs args) { Debug.Log("OnAdLoaded"); }
	void OnAdFailedToLoad(object sender, AdFailedToLoadEventArgs e) { Debug.Log("OnAdFailedToLoad"); }
	void OnAdOpening(object sender, EventArgs e) { Debug.Log("OnAdOpening"); }
	void OnAdStarted(object sender, EventArgs e) { Debug.Log("OnAdStarted"); }

	void OnAdRewarded(object sender, Reward e)
	{
		Debug.Log("OnAdRewarded");
	}

	void OnAdClosed(object sender, EventArgs e)
	{
		Debug.Log("OnAdClosed");
	}

	void OnAdLeavingApplication(object sender, EventArgs e) { Debug.Log("OnAdLeavingApplication"); }

	public void Show()
	{
		InitAd();

		StartCoroutine("ShowScreenAd");
	}

	private IEnumerator ShowScreenAd()
	{
		while (!rewardAd.IsLoaded())
		{
			yield return null;
		}

		rewardAd.Show();
	}
}
