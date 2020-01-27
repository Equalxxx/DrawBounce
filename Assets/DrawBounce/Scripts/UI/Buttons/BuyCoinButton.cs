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
		if (GameManager.Instance.IsAddCoin(addCoin))
		{
			if(GameManager.IsConnected)
			{
				if(!GameManager.IsPracticeMode)
				{
					IAPManager.Instance.Purchase(targetProductId);
					SoundManager.Instance.PlaySound2D("Buy_Item");
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
		if(success)
		{
			if(string.Equals(productId, targetProductId))
			{
				GameManager.Instance.AddCoin(addCoin);
				AddCoinEffect coinEffect = PoolManager.Instance.Spawn("AddCoinEffect", Camera.main.transform.position, Quaternion.identity).GetComponent<AddCoinEffect>();
				coinEffect.RefreshEffect(addCoin);
			}
		}
		else
		{
			Debug.Log("Purchase Failed");
		}
	}
}
