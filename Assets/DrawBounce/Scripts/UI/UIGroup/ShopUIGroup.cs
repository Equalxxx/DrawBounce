using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShopTapType { Coin, Block, BG, Gem, Help, Ads }
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

	public override void InitUI()
	{
		if(GameManager.IsNoAds || GameManager.IsOfflineMode)
		{
			for (int i = 0; i < tapButtons.Length; i++)
			{
				if(tapButtons[i].shopTapType == ShopTapType.Ads)
				{
					if(tapButtons[i].gameObject.activeSelf)
						tapButtons[i].gameObject.SetActive(false);
					break;
				}
			}

			curTapType = ShopTapType.Block;
		}
	}

	public override void RefreshUI()
	{
		for (int i = 0; i < tapButtons.Length; i++)
		{
			if(tapButtons[i].gameObject.activeSelf)
				tapButtons[i].RefreshUI(curTapType);
		}

		for(int i = 0; i < scrollViews.Length; i++)
		{
			scrollViews[i].RefreshUI(curTapType);
		}
	}
}
