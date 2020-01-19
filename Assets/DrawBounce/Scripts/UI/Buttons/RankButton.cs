using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RankButton : BasicUIButton
{
	protected override void InitButton()
	{
	}

	protected override void PressedButton()
	{
		if (GooglePlayManager.IsConnected)
			GooglePlayManager.Instance.ShowLeaderboardUI();
		else
			UIManager.Instance.notConnectedUI.Show();
	}
}
