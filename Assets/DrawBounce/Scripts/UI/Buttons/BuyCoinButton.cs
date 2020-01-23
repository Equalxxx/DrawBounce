using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MysticLights;

public class BuyCoinButton : BasicUIButton
{
	public TextMeshProUGUI coinValueText;
	public TextMeshProUGUI gemValueText;

	public string targetProductId;
	public int price = 100;
	public int addCoin = 1000;

	protected override void OnEnable()
	{
		base.OnEnable();
		IAPManager.PuchaseCompleteAction += PurchaseComplete;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		IAPManager.PuchaseCompleteAction -= PurchaseComplete;
	}

	protected override void InitButton()
	{
		RefreshUI();
	}

	void RefreshUI()
	{
		coinValueText.text = addCoin.ToString();
		gemValueText.text = price.ToString();
	}

	protected override void PressedButton()
	{
		if (targetProductId == IAPManager.ProductSkin)
		{
			if (IAPManager.Instance.HadPurchased(targetProductId))
			{
				Debug.LogFormat("Had product : {0}", targetProductId);
				return;
			}
		}

		if (GameManager.Instance.IsAddCoin(addCoin))
		{
			if(GameManager.IsInternetConnected && GooglePlayManager.IsAuthenticated)
			{
				IAPManager.Instance.Purchase(targetProductId);
				SoundManager.Instance.PlaySound2D("Buy_Item");
			}
			else
			{
				if(Debug.isDebugBuild)
				{
					Debug.Log("debug purchase");
					IAPManager.Instance.Purchase(targetProductId);
					SoundManager.Instance.PlaySound2D("Buy_Item");
				}
				else
				{
					UIManager.Instance.showMessageUI.Show(10);
					SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
				}
			}
		}
		else
		{
			SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
		}
	}

	void PurchaseComplete(bool success, string productId)
	{
		if(success)
		{
			if(string.Equals(productId, targetProductId))
			{
				GameManager.Instance.AddCoin(addCoin);
				GameManager.Instance.gameSettings.SaveGameInfo();
			}
		}
		else
		{
			UIManager.Instance.showMessageUI.Show(10);
			Debug.Log("Purchase Failed");
		}
	}
}
