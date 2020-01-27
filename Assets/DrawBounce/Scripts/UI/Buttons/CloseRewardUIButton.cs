using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseRewardUIButton : BasicUIButton
{
	private RewardUI rewardUI;

	protected override void InitButton()
	{
		if (rewardUI == null)
			rewardUI = GetComponentInParent<RewardUI>();
	}

	protected override void PressedButton()
	{
		rewardUI.Show(false);
	}
}
