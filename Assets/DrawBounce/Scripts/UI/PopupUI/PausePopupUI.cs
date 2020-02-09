using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausePopupUI : ProtoPopupUI
{
	public override void InitPopupUI()
	{
		popupUIType = PopupUIType.Pause;

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
