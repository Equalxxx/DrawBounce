using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MysticLights;

public class BuyHeightButton : BasicUIButton
{
	public TextMeshProUGUI heightText;
	public TextMeshProUGUI priceText;

	public int price = 100;
	public float addHeight = 100f;

	public Color onColor = Color.white;
	public Color offColor = Color.white;

	protected override void InitButton()
	{

	}

	protected override void OnEnable()
	{
		base.OnEnable();
		GameManager.GameInitAction += RefreshUI;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.GameInitAction -= RefreshUI;
	}

	void RefreshUI()
	{
		heightText.text = UnitCalculation.GetHeightText(GameManager.Instance.gameInfo.startHeight);
		priceText.text = UnitCalculation.GetCoinText(GetPrice());

		if (GameManager.Instance.IsAddHeight(100f))
		{
			priceText.color = onColor;
		}
		else
		{
			priceText.color = offColor;
		}
	}

	protected override void PressedButton()
	{
		int useCoin = GetPrice();

		if (GameManager.Instance.IsUseCoin(useCoin) && GameManager.Instance.IsAddHeight(addHeight))
		{
			GameManager.Instance.UseCoin(useCoin);
			GameManager.Instance.AddHeight(addHeight);

			GameManager.Instance.gameSettings.SaveGameInfo();

			RefreshUI();
		}
		else
		{
			SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
		}
	}

	int GetPrice()
	{
		int height = (int)(GameManager.Instance.gameInfo.startHeight / addHeight) + 1;
		if (height <= 0)
			height = 1;

		return price * height;
	}
}
