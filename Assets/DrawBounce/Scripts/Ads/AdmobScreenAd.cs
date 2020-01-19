using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdmobScreenAd : MonoBehaviour
{
	private readonly string unitId = "ca-app-pub-2141587155447938/6875380797";

	private readonly string test_unitId = "ca-app-pub-3940256099942544/8691691433";
	private readonly string test_deviceId = "";

	private InterstitialAd screenAd;

	private void InitAd()
	{
		string id = Debug.isDebugBuild ? test_unitId : unitId;

		screenAd = new InterstitialAd(id);

		AdRequest request = new AdRequest.Builder().AddTestDevice(test_deviceId).Build();

		screenAd.LoadAd(request);

		screenAd.OnAdClosed += (sender, e) => Debug.Log("Ad closed");
		screenAd.OnAdLoaded += (sender, e) => Debug.Log("Ad loaded");
	}

	public void Show()
	{
		InitAd();

		StartCoroutine("ShowScreenAd");
	}

	private IEnumerator ShowScreenAd()
	{
		while(!screenAd.IsLoaded())
		{
			yield return null;
		}

		screenAd.Show();
	}
}
