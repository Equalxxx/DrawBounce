using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using TMPro;
using MLFramework;

public class PurchaseButton : BasicUIButton
{
	public enum PurchaseProductType { Coin, NoAds }
	public PurchaseProductType productType;
	public string targetProductId;
	public int addValue;

	public TextMeshProUGUI productTitleText;
	public TextMeshProUGUI productPriceText;
	public TextMeshProUGUI productValueText;
	public TextMeshProUGUI productDescText;

	private Product product;

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
		if (GameManager.IsOfflineMode)
			return;

		if (product == null)
		{
			product = IAPManager.Instance.GetProductData(targetProductId);
			if (product == null)
				return;
		}

		if (productTitleText)
			productTitleText.text = product.metadata.localizedTitle;

		if (productPriceText)
			productPriceText.text = product.metadata.localizedPriceString;

		if (productDescText)
			productDescText.text = product.metadata.localizedDescription;

		if (productValueText)
			productValueText.text = addValue.ToString();
	}

	protected override void PressedButton()
	{
		switch(productType)
		{
			case PurchaseProductType.Coin:
				if (!GameManager.Instance.IsAddCoin(addValue))
				{
					UIManager.Instance.showMessageUI.Show(11);
					SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
					return;
				}
				break;
			case PurchaseProductType.NoAds:
				break;
		}

		if (GameManager.IsConnected)
		{
			IAPManager.Instance.Purchase(targetProductId);
		}
		else
		{
			UIManager.Instance.showMessageUI.Show(10);
			SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
		}
	}

	void PurchaseComplete(bool success, string productId)
	{
		if (success)
		{
			Debug.LogFormat("equal? : {0}, {1}", productId, targetProductId);
			if (string.Equals(productId, targetProductId))
			{
				switch(productType)
				{
					case PurchaseProductType.Coin:
						GameManager.Instance.AddCoin(addValue);
						AddCoinEffect coinEffect = PoolManager.Instance.Spawn("AddCoinEffect", Camera.main.transform.position, Quaternion.identity).GetComponent<AddCoinEffect>();
						SoundManager.Instance.PlaySound2D("AddCoin");
						coinEffect.RefreshEffect(addValue);
						GameManager.Instance.gameSettings.SaveGameData();
						break;
					case PurchaseProductType.NoAds:
						UIManager.Instance.ShowPopup(PopupUIType.NoAds, false);
						GameManager.Instance.CheckNoAds();
						if(GameManager.Instance.gameState != GameState.GameTitle)
							GameManager.Instance.SetGameState(GameState.GameTitle);
						break;
				}
			}
		}
		else
		{
			Debug.LogWarningFormat("Purchase Failed : {0}", targetProductId);
		}
	}
}