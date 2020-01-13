using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUIGroup : UIGroup
{
	public TextMeshProUGUI maxMeterText;

	private void OnValidate()
	{
		groupType = UIGroupType.Result;
	}

	private void OnEnable()
	{
		GameManager.GameOverAction += RefreshUI;
	}

	private void OnDisable()
	{
		GameManager.GameOverAction -= RefreshUI;
	}

	void RefreshUI()
	{
		maxMeterText.text = GetHeightText(GameManager.Instance.player.GetLastHeight());
	}

	string GetHeightText(float height)
	{
		string distText = "";

		if (height >= 1000f)
		{
			float kilo = height / 1000f;
			distText = string.Format("{0:f2}KM", kilo);
		}
		else
		{
			distText = string.Format("{0:f2}M", height);
		}

		return distText;
	}
}
