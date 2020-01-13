using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyHeightButton : BasicUIButton
{
	public TextMeshProUGUI heightText;
	public TextMeshProUGUI priceText;
	public int price = 100;
	public float addHeight = 100f;

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
	}

	int GetPrice()
	{
		int height = (int)(GameManager.Instance.gameInfo.startHeight / addHeight) + 1;
		if (height <= 0)
			height = 1;

		return price * height;
	}
}
