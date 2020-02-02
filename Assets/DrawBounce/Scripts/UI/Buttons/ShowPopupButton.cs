using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPopupButton : BasicUIButton
{
	public PopupUIType popupUIType;

	protected override void PressedButton()
	{
		UIManager.Instance.ShowPopup(popupUIType, true);
	}
}
