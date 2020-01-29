using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAdsButton : BasicUIButton
{
	public bool show;

	protected override void PressedButton()
	{
		UIManager.Instance.ShowNoAdsUI(show);
	}
}
