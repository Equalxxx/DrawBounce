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
		GameSettings.GameSettingAction += RefreshUI;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.GameInitAction -= RefreshUI;
		GameSettings.GameSettingAction -= RefreshUI;
	}

	void RefreshUI()
	{
		GameManager gameManager = GameManager.Instance;

		hpText.text = gameManager.gameInfo.playerHP.ToString();
		priceText.text = UnitCalculation.GetCoinText(GetPrice());

		if (gameManager.IsAddHP(1) && gameManager.curPlayableBlock.blockType != PlayableBlockType.CoinCircle)
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
		GameManager gameManager = GameManager.Instance;

		if (gameManager.IsUseCoin(useCoin) && gameManager.IsAddHP(addHp))
		{
			if(gameManager.curPlayableBlock.blockType != PlayableBlockType.CoinCircle)
			{
				gameManager.UseCoin(useCoin);
				gameManager.AddHP(addHp);
				RefreshUI();
			}
			else
				SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
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
