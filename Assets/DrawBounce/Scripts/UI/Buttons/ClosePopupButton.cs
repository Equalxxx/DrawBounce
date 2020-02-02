using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopupButton : BasicUIButton
{
	protected override void PressedButton()
	{
		MysticLights.PopupUIManager.Instance.ClosePopupUI();
	}
}
