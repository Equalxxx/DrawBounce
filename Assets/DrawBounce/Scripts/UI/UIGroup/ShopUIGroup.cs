using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopTapType { Coin, BLK, BG, Gem, Help }
public class ShopUIGroup : UIGroup
{
	public ShopTapType curTapType;

	public Color tapEnableColor = Color.white;
	public Color tapDisableColor = Color.white;

	public ShopTapButton[] tapButtons;
	public ShopScrollView[] scrollViews;

	private void OnValidate()
	{
		groupType = UIGroupType.Shop;
	}

	protected override void InitUI()
	{
	}

	public override void RefreshUI()
	{
		for (int i = 0; i < tapButtons.Length; i++)
		{
			tapButtons[i].RefreshUI(curTapType);
		}

		for(int i = 0; i < scrollViews.Length; i++)
		{
			scrollViews[i].RefreshUI(curTapType);
		}
	}
}
