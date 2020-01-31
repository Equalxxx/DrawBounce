using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public class BuyNoAdsButton : BasicUIButton
{
	public string targetProductId = "noadspackage";
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

	protected override void PressedButton()
	{
		if (GameManager.Instance.IsAddCoin(addCoin))
		{
			if (GameManager.IsConnected)
			{
				if (!GameManager.IsPracticeMode)
				{
					IAPManager.Instance.Purchase(targetProductId);
				}
				else
				{
					UIManager.Instance.showMessageUI.Show(12);
					SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
				}
			}
			else
			{
				UIManager.Instance.showMessageUI.Show(10);
				SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
			}
		}
		else
		{
			UIManager.Instance.showMessageUI.Show(11);
			SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
		}
	}

	void PurchaseComplete(bool success, string productId)
	{
		if (success)
		{
			if (string.Equals(productId, targetProductId))
			{
				GameManager.Instance.AddCoin(addCoin);
				AddCoinEffect coinEffect = PoolManager.Instance.Spawn("AddCoinEffect", Camera.main.transform.position, Quaternion.identity).GetComponent<AddCoinEffect>();
				SoundManager.Instance.PlaySound2D("AddCoin");
				coinEffect.RefreshEffect(addCoin);
				GameManager.Instance.gameSettings.SaveInfoToServer();

				GameManager.Instance.CheckNoAds();
			}
		}
		else
		{
			Debug.LogFormat("Purchase Failed : {0}", targetProductId);
		}
	}
}
