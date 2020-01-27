using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public class RankButton : BasicUIButton
{
	protected override void PressedButton()
	{
		if (GameManager.IsConnected)
		{
			if(!GameManager.IsPracticeMode)
			{
				GooglePlayManager.Instance.ShowLeaderboardUI();
			}
			else
			{
				UIManager.Instance.showMessageUI.Show(12);
			}
		}
		else
			UIManager.Instance.showMessageUI.Show(10);
	}
}
