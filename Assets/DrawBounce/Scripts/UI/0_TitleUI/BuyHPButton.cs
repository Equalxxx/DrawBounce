using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuyHPButton : MonoBehaviour
{
	private Button myButton;
	private TextMeshProUGUI hpText;
	public int useScore = 30;
	public int addHp = 1;

	private void Awake()
	{
		myButton = GetComponent<Button>();
		hpText = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		myButton.onClick.AddListener(PressedButton);
		GameManager.GameInitAction += RefreshUI;
	}

	private void OnDisable()
	{
		myButton.onClick.RemoveListener(PressedButton);
		GameManager.GameInitAction -= RefreshUI;
	}

	void RefreshUI()
	{
		hpText.text = string.Format("x {0}", GameManager.Instance.gameInfo.playerHP);
	}

	void PressedButton()
	{
		if(GameManager.Instance.IsUseScore(useScore) && GameManager.Instance.IsAddPlayerHP(addHp))
		{
			GameManager.Instance.UseScore(useScore);
			GameManager.Instance.AddPlayerHP(addHp);

			GameManager.Instance.gameSettings.SaveGameInfo();

			RefreshUI();
		}
	}
}
