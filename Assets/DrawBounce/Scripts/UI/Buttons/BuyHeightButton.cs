using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BuyHeightButton : BasicUIButton
{
	private TextMeshProUGUI heightText;
	public int useCoin = 0;
	public float addHeight = 100f;

	protected override void InitButton()
	{
		heightText = GetComponentInChildren<TextMeshProUGUI>();
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
		heightText.text = GetHeightText(GameManager.Instance.gameInfo.startHeight);
	}

	protected override void PressedButton()
	{
		if (GameManager.Instance.IsUseCoin(useCoin) && GameManager.Instance.IsAddStartHeight(addHeight))
		{
			GameManager.Instance.UseCoin(useCoin);
			GameManager.Instance.AddStartHeight(addHeight);

			GameManager.Instance.gameSettings.SaveGameInfo();

			RefreshUI();
		}
	}

	string GetHeightText(float height)
	{
		string distText = "";

		if (height >= 1000f)
		{
			float kilo = height / 1000f;
			distText = string.Format("{0:f1}KM", kilo);
		}
		else
		{
			int meter = (int)height;
			distText = string.Format("{0}M", meter);
		}

		return distText;
	}
}
