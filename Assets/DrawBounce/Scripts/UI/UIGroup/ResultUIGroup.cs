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
		maxMeterText.text = GetMeterText(GameManager.Instance.player.GetLastHeight());
	}

	string GetMeterText(float height)
	{
		return string.Format("{0:f1}M", height);
	}
}
