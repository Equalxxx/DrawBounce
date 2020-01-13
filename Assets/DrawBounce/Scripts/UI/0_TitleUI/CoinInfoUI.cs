using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoinInfoUI : MonoBehaviour
{
	private TextMeshProUGUI coinText;

	private void Awake()
	{
		coinText = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		GameManager.GameInitAction += RefreshUI;
		GameManager.UseCoinAction += RefreshUI;
		GameManager.AddCoinAction += RefreshUI;
	}

	private void OnDisable()
	{
		GameManager.GameInitAction -= RefreshUI;
		GameManager.UseCoinAction -= RefreshUI;
		GameManager.AddCoinAction -= RefreshUI;
	}

	void RefreshUI()
	{
		coinText.text = GetCoinText();
	}

	string GetCoinText()
	{
		int coin = GameManager.Instance.gameInfo.coin;
		string unit = "";

		if(coin > 1000000000)
		{
			unit = "G";
		}
		else if(coin > 1000000)
		{
			unit = "M";
		}
		else if(coin > 1000)
		{
			unit = "K";
		}

		return string.Format("x {0}{1}", coin, unit);

	}
}
