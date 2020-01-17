using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdMobAdsManager : MonoBehaviour
{
	private InterstitialAd interstitial;

	public void Start()
	{
#if UNITY_ANDROID
		string appId = "ca-app-pub-3940256099942544~3347511713";
#elif UNITY_IPHONE
            string appId = "ca-app-pub-3940256099942544~1458002511";
#else
            string appId = "unexpected_platform";
#endif
		// Initialize the Google Mobile Ads SDK.
		MobileAds.Initialize(appId);

		this.RequestInterstitial();

		StartCoroutine(DelayStartAds());
	}

	IEnumerator DelayStartAds()
	{
		yield return new WaitForSeconds(1f);
		ShowInterstitial();
	}

	// 전면 광고 
	private void RequestInterstitial()
	{
#if UNITY_ANDROID
		string adUnitId = "ca-app-pub-3940256099942544/1033173712"; //테스트 아이디 
#elif UNITY_IPHONE
        //string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

		// Initialize an InterstitialAd.
		this.interstitial = new InterstitialAd(adUnitId);
		// 전면광고
		// Called when an ad request has successfully loaded.
		this.interstitial.OnAdLoaded += HandleOnAdLoaded;
		// Called when an ad request failed to load.
		this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is shown.
		this.interstitial.OnAdOpening += HandleOnAdOpened;
		// Called when the ad is closed.
		this.interstitial.OnAdClosed += HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
		this.interstitial.LoadAd(request);
	}

	public void HandleOnAdLoaded(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLoaded event received");
	}

	public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
	{
		MonoBehaviour.print("interstitial Failed : "
							+ args.Message);
	}

	public void HandleOnAdOpened(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdOpened event received");
	}

	public void HandleOnAdClosed(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdClosed event received");
		RequestInterstitial();
	}

	public void HandleOnAdLeavingApplication(object sender, EventArgs args)
	{
		MonoBehaviour.print("HandleAdLeavingApplication event received");
	}

	public void ShowInterstitial()
	{
		if (this.interstitial.IsLoaded())
		{
			this.interstitial.Show();
		}
		else
		{
			Debug.Log("NOT Loaded Interstitial");
			RequestInterstitial();
		}
	}
}
