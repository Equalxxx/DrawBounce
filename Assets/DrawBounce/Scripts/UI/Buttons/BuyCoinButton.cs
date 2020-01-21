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
		//int useGem = price;

		//if (GameManager.Instance.AddCoin(addCoin))
		//{
		//	GameManager.Instance.gameSettings.SaveGameInfo();

		//	RefreshUI();
		//}

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
			if(GooglePlayManager.IsConnected)
			{
				IAPManager.Instance.Purchase(targetProductId, PurchaseComplete);
				SoundManager.Instance.PlaySound2D("Buy_Item");
			}
			else
			{
				if(Debug.isDebugBuild)
				{
					Debug.Log("debug purchase");
					IAPManager.Instance.Purchase(targetProductId, PurchaseComplete);
					SoundManager.Instance.PlaySound2D("Buy_Item");
				}
				else if(GameManager.Instance.testMode)
				{
					GameManager.Instance.AddCoin(addCoin);
					GameManager.Instance.gameSettings.SaveGameInfo();
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

	void PurchaseComplete(bool success)
	{
		if(success)
		{
			GameManager.Instance.AddCoin(addCoin);
			GameManager.Instance.gameSettings.SaveGameInfo();
		}
		else
		{
			UIManager.Instance.showMessageUI.Show(10);
		}
	}
}
