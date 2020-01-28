using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobInterstitialAd : MonoBehaviour
{
	private readonly string unitId = "ca-app-pub-2141587155447938/6875380797";

	private readonly string test_unitId = "ca-app-pub-3940256099942544/8691691433";
	private readonly string test_deviceId = "";

	private InterstitialAd interstitialAd;

	private void Start()
	{
		interstitialAd = CreateAndLoadInterstitialAd();
	}

	InterstitialAd CreateAndLoadInterstitialAd()
	{
		string adUnitId = test_unitId;

		InterstitialAd ad = new InterstitialAd(test_unitId);

		// Called when an ad request has successfully loaded.
		ad.OnAdLoaded += HandleOnAdLoaded;
		// Called when an ad request failed to load.
		ad.OnAdFailedToLoad += HandleOnAdFailedToLoad;
		// Called when an ad is shown.
		ad.OnAdOpening += HandleOnAdOpened;
		// Called when the ad is closed.
		ad.OnAdClosed += HandleOnAdClosed;
		// Called when the ad click caused the user to leave the application.
		ad.OnAdLeavingApplication += HandleOnAdLeavingApplication;

		// Create an empty ad request.
		AdRequest request = new AdRequest.Builder().Build();
		// Load the interstitial with the request.
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

	public void ShowAd()
	{
		if (interstitialAd.IsLoaded())
		{
			interstitialAd.Show();
		}
	}
}
