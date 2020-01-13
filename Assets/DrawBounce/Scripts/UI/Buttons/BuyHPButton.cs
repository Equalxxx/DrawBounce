using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyHPButton : BasicUIButton
{
	private TextMeshProUGUI hpText;
	public int useCoin = 30;
	public int addHp = 1;

	protected override void InitButton()
	{
		hpText = GetComponentInChildren<TextMeshProUGUI>();
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
		hpText.text = string.Format("x {0}", GameManager.Instance.gameInfo.playerHP);
	}

	protected override void PressedButton()
	{
		if(GameManager.Instance.IsUseCoin(useCoin) && GameManager.Instance.IsAddPlayerHP(addHp))
		{
			GameManager.Instance.UseCoin(useCoin);
			GameManager.Instance.AddPlayerHP(addHp);

			GameManager.Instance.gameSettings.SaveGameInfo();

			RefreshUI();
		}
	}
}
