using System.Collections;
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
		int height = (int)GameManager.Instance.player.GetLastHeight();
		int startHeight = (int)GameManager.Instance.lastStartHeight;

		if (height - startHeight >= limitHeight)
		{
			rewardUI.Show(true);
			rewardUI.InitUI();
		}
		else
		{
			rewardUI.Show(false);
			AdmobManager.Instance.ShowAd(AdmobAdType.Interstitial);
		}
	}

	public override void RefreshUI()
	{
		recordText.text = UnitCalculation.GetHeightText(GameManager.Instance.gameInfo.lastHeight, true);
		maxMeterText.text = UnitCalculation.GetHeightText(GameManager.Instance.player.GetLastHeight(), true);
	}
}
