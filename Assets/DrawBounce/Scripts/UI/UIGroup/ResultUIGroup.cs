using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUIGroup : UIGroup
{
	public TextMeshProUGUI recordText;
	public TextMeshProUGUI maxMeterText;
	//public int minLimitHeight = 50;
	//public int maxLimitHeight = 100;
	//public GameObject retryButton;

	private void OnValidate()
	{
		groupType = UIGroupType.Result;
	}

	private void OnEnable()
	{
		GameManager.GameOverAction += RefreshUI;
		//AdmobManager.AdInterstitialAction += ClosedAd;
	}

	private void OnDisable()
	{
		GameManager.GameOverAction -= RefreshUI;
		//AdmobManager.AdInterstitialAction -= ClosedAd;
	}

	public override void InitUI()
	{
//		int height = (int)GameManager.Instance.player.GetLastHeight();
//		int startHeight = (int)GameManager.Instance.gameInfo.startHeight;

//		if (height - startHeight >= minLimitHeight && height - startHeight <= maxLimitHeight)
//		{
//			ShowRetryButton(false);
//			ShowAd();
//		}
//		else
//		{
//			ShowRetryButton(true);
//		}

//#if UNITY_EDITOR
//		ShowRetryButton(true);
//#endif
	}

	public override void RefreshUI()
	{
		recordText.text = UnitCalculation.GetHeightText(GameManager.Instance.gameInfo.lastHeight, true);
		maxMeterText.text = UnitCalculation.GetHeightText(GameManager.Instance.player.GetLastHeight(), true);
	}

	//void ShowAd()
	//{
	//	AdmobManager.Instance.ShowAd(AdmobAdType.Interstitial);
	//}

	//void ClosedAd()
	//{
	//	ShowRetryButton(true);
	//}

	//void ShowRetryButton(bool show)
	//{
	//	if (retryButton.activeSelf != show)
	//		retryButton.SetActive(show);
	//}
}
