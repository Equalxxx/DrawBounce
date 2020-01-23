using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public class RankButton : BasicUIButton
{
	protected override void PressedButton()
	{
		if (GameManager.IsInternetConnected && GooglePlayManager.IsAuthenticated)
			GooglePlayManager.Instance.ShowLeaderboardUI();
		else
			UIManager.Instance.showMessageUI.Show(10);
	}
}
