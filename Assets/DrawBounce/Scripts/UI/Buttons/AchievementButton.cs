using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementButton : BasicUIButton
{
	protected override void InitButton()
	{
	}

	protected override void PressedButton()
	{
		if (GameManager.IsInternetConnected && GooglePlayManager.IsAuthenticated)
			GooglePlayManager.Instance.ShowAchievementUI();
		else
			UIManager.Instance.showMessageUI.Show(10);
	}
}