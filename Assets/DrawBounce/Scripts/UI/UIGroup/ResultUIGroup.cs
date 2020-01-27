using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUIGroup : UIGroup
{
	public TextMeshProUGUI recordText;
	public TextMeshProUGUI maxMeterText;
	public RewardUI rewardUI;

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
		rewardUI.InitUI();
	}

	public override void RefreshUI()
	{
		recordText.text = UnitCalculation.GetHeightText(GameManager.Instance.gameInfo.lastHeight, true);
		maxMeterText.text = UnitCalculation.GetHeightText(GameManager.Instance.player.GetLastHeight(), true);
	}
}
