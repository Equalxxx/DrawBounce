using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BuyCoinButton : BasicUIButton
{
	public TextMeshProUGUI coinValueText;
	public TextMeshProUGUI gemValueText;
	public int price = 100;
	public int addCoin = 1000;
	public DOTweenAnimation buttonAnim;

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
		int useGem = price;

		if (GameManager.Instance.AddCoin(addCoin))
		{
			GameManager.Instance.gameSettings.SaveGameInfo();

			RefreshUI();
		}

		buttonAnim.DORestart();
	}
}
