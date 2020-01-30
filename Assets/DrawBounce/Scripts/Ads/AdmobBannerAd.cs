using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobBannerAd : MonoBehaviour
{
	private readonly string unitId = "ca-app-pub-2141587155447938/6875380797";

	private readonly string test_unitId = "ca-app-pub-3940256099942544/6300978111";

	private BannerView bannerAd;

	private void Start()
	{
		bannerAd = CreateAndLoadBannerAd();
		bannerAd.Hide();
	}

	BannerView CreateAndLoadBannerAd()
	{
		//AdSize adSize = AdSize.GetPortraitAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);
		//BannerView banner = new BannerView(test_unitId, adSize, AdPosition.Top);
		BannerView banner = new BannerView(test_unitId, AdSize.SmartBanner, AdPosition.Top);

		// Called when an ad request has successfully loaded.
		banner.OnAdLoaded += HandleOnAdLoaded_banner;
		// Called when an ad request failed to load.
		banner.OnAdFailedToLoad += HandleOnAdFailedToLoad_banner;
		// Called when an ad is clicked.
		banner.OnAdOpening += HandleOnAdOpened_banner;
		// Called when the user returned from the app after an ad click.
		banner.OnAdClosed += HandleOnAdClosed_banner;
		// Called when the ad click caused the user to leave the application.
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

	public void ShowAd()
	{
		bannerAd.Show();
	}

	public void HideAd()
	{
		bannerAd.Hide();
	}

	public float GetWidth()
	{
		return bannerAd.GetWidthInPixels();
	}

	public float GetHeight()
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
