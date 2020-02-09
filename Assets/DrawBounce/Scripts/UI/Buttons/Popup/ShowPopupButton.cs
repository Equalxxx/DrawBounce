using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowPopupButton : BasicUIButton
{
	public PopupUIType popupUIType;
	public bool close;

	protected override void PressedButton()
	{
		if (close)
			UIManager.Instance.ShowPopup(PopupUIType.Option, false);

		UIManager.Instance.ShowPopup(popupUIType, true);
	}
}
