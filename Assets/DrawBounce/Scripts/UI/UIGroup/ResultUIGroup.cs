﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUIGroup : UIGroup
{
	public TextMeshProUGUI recordText;
	public TextMeshProUGUI maxMeterText;
	public RewardUI rewardUI;
	public int limitHeight = 50;

	private void OnValidate()
	{
		groupType = UIGroupType.Result;
	}

	private void OnEnable()
	{
		GameManager.GameOverAction += RefreshUI;
	}

	private void OnDisable()
	{
		GameManager.GameOverAction -= RefreshUI;
	}

	public override void InitUI()
	{
		int height = (int)GameManager.Instance.curPlayableBlock.GetLastHeight();
		int startHeight = (int)GameManager.Instance.lastStartHeight;

		if(GameManager.Instance.gameInfo.lastHeight > 100f && GameManager.Instance.deviceSettings.review)
		{
			UIManager.Instance.ShowPopup(PopupUIType.Review, true);
			GameManager.Instance.deviceSettings.review = false;
			GameManager.Instance.gameSettings.SaveDeviceOptions();
		}

		if (height - startHeight >= limitHeight && !GameManager.IsOfflineMode)
		{
			rewardUI.Show(true);
			rewardUI.InitUI();
		}
		else
		{
			rewardUI.Show(false);
			//if(!GameManager.IsNoAds)
			//	AdmobManager.Instance.ShowAd(AdmobAdType.Interstitial);
		}
	}

	public override void RefreshUI()
	{
		recordText.text = UnitCalculation.GetHeightText(GameManager.Instance.gameInfo.lastHeight, true);
		maxMeterText.text = UnitCalculation.GetHeightText(GameManager.Instance.curPlayableBlock.GetLastHeight(), true);
	}
}
