using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementButton : BasicUIButton
{
	protected override void PressedButton()
	{
		if (GameManager.IsConnected)
			GooglePlayManager.Instance.ShowAchievementUI();
		else
			UIManager.Instance.showMessageUI.Show(10);
	}
}