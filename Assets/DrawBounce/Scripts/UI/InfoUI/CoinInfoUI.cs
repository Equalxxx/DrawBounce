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
		GameSettings.GameSettingAction += RefreshUI;
	}

	private void OnDisable()
	{
		GameManager.GameInitAction -= RefreshUI;
		GameSettings.GameSettingAction -= RefreshUI;
	}

	public void RefreshUI()
	{
		coinText.text = GameManager.Instance.gameInfo.coin.ToString();
	}
}
