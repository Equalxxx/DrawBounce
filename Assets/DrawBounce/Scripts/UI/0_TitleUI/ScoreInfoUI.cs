using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreInfoUI : MonoBehaviour
{
	private TextMeshProUGUI scoreText;

	private void Awake()
	{
		scoreText = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		GameManager.GameInitAction += RefreshUI;
		GameManager.UseScoreAction += RefreshUI;
		GameManager.AddScoreAction += RefreshUI;
	}

	private void OnDisable()
	{
		GameManager.GameInitAction -= RefreshUI;
		GameManager.UseScoreAction -= RefreshUI;
		GameManager.AddScoreAction -= RefreshUI;
	}

	void RefreshUI()
	{
		scoreText.text = GetScoreText();
	}

	string GetScoreText()
	{
		int score = GameManager.Instance.gameInfo.score;
		string unit = "";

		if(score > 1000000000)
		{
			unit = "G";
		}
		else if(score > 1000000)
		{
			unit = "M";
		}
		else if(score > 1000)
		{
			unit = "K";
		}

		return string.Format("x {0}{1}", score, unit);

	}
}
