using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GemInfoUI : MonoBehaviour
{
	private TextMeshProUGUI gemText;

	private void Awake()
	{
		gemText = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void OnEnable()
	{
		GameManager.GameInitAction += RefreshUI;
		GameManager.UseGemAction += RefreshUI;
		GameManager.AddGemAction += RefreshUI;
	}

	private void OnDisable()
	{
		GameManager.GameInitAction -= RefreshUI;
		GameManager.UseGemAction -= RefreshUI;
		GameManager.AddGemAction -= RefreshUI;
	}

	void RefreshUI()
	{
		gemText.text = GameManager.Instance.gameInfo.gem.ToString();
	}
}
