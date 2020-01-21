using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MysticLights;

public class BuyHPButton : BasicUIButton
{
	public TextMeshProUGUI hpText;
	public TextMeshProUGUI priceText;

	public int price = 30;
	public int addHp = 1;

	public Color onColor = Color.white;
	public Color offColor = Color.white;

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
		hpText.text = GameManager.Instance.gameInfo.playerHP.ToString();
		priceText.text = UnitCalculation.GetCoinText(GetPrice());

		if (GameManager.Instance.IsAddHP(1))
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

		if (GameManager.Instance.IsUseCoin(useCoin) && GameManager.Instance.IsAddHP(addHp))
		{
			GameManager.Instance.UseCoin(useCoin);
			GameManager.Instance.AddHP(addHp);

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
		return price * GameManager.Instance.gameInfo.playerHP;
	}
}
