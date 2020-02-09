using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosePopupButton : BasicUIButton
{
	protected override void PressedButton()
	{
		MLFramework.PopupUIManager.Instance.ClosePopupUI();
	}
}
