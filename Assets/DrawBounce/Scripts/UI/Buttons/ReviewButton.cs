using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewButton : BasicUIButton
{
	protected override void PressedButton()
	{
		Application.OpenURL("https://play.google.com/store/apps/details?id=com.mysticlights.drawbounce");

		UIManager.Instance.ShowPopup(PopupUIType.Review, false);
	}
}
