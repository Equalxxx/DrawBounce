using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopTapButton : BasicUIButton
{
	public ShopTapType shopTapType;
	
	private Image buttonImage;
	private ShopUIGroup shopUIGroup;

	protected override void InitButton()
	{
		if (shopUIGroup == null)
			shopUIGroup = GetComponentInParent<ShopUIGroup>();
		if (buttonImage == null)
			buttonImage = GetComponent<Image>();
	}

	protected override void PressedButton()
	{
		shopUIGroup.curTapType = shopTapType;
		shopUIGroup.RefreshUI();
	}

	public void RefreshUI(ShopTapType tapType)
	{
		if(shopTapType == tapType)
		{
			buttonImage.color = shopUIGroup.tapEnableColor;
		}
		else
		{
			buttonImage.color = shopUIGroup.tapDisableColor;
		}
	}
}
