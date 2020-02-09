﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitPopupUI : ProtoPopupUI
{
	public override void InitPopupUI()
	{
		popupUIType = PopupUIType.Quit;

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
