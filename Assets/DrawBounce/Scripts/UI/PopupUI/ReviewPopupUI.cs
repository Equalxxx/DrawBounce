using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReviewPopupUI : ProtoPopupUI
{
	public override void InitPopupUI()
	{
		popupUIType = PopupUIType.Review;

		Time.timeScale = 0f;
	}

	public override void RefreshUI()
	{
	}

	public override void ClosePopupUI()
	{
		Time.timeScale = 1f;
	}
}
