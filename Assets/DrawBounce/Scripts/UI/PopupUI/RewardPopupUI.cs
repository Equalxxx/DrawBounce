﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardPopupUI : ProtoPopupUI
{
	public int limitHeight = 50;
	public int addHeight = 100;
	public int addCoinValue;
	private RewardButton rewardButton;

	public override void InitPopupUI()
	{
		if (GameManager.Instance.curPlayableBlock == null)
			return;

		Debug.Log("Reward Init");

		if (rewardButton == null)
			rewardButton = GetComponentInChildren<RewardButton>();

		int height = (int)GameManager.Instance.curPlayableBlock.GetLastHeight();

		if (height >= 100)
		{
			addCoinValue = GetRewardValue(height);
		}
		else
		{
			addCoinValue = 100;
		}

		rewardButton.coinText.text = addCoinValue.ToString();
	}

	public override void ClosePopupUI()
	{

	}

	public override void RefreshUI()
	{

	}

	int GetRewardValue(int height)
	{
		int amount = height / addHeight;

		return (int)(amount * addHeight * (GameManager.Instance.curPlayableBlock.addHeightCoinPer/100f) * ((float)GameManager.Instance.curTargetHeight.level/2f));
	}
}
