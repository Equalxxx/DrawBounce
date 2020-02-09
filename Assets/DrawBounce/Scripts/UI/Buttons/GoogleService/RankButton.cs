using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLFramework;

public class RankButton : BasicUIButton
{
	protected override void PressedButton()
	{
		if (GameManager.IsConnected)
			GooglePlayManager.Instance.ShowLeaderboardUI();
		else
			UIManager.Instance.showMessageUI.Show(10);
	}
}
