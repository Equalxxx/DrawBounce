using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyHPButton : BasicUIButton
{
	public TextMeshProUGUI hpText;
	public TextMeshProUGUI priceText;
	public int price = 30;
	public int addHp = 1;

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
		hpText.text = GameManager.Instance.gameInfo.playerHP.ToString();
		priceText.text = UnitCalculation.GetCoinText(GetPrice());
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
	}

	int GetPrice()
	{
		return price * GameManager.Instance.gameInfo.playerHP;
	}
}
