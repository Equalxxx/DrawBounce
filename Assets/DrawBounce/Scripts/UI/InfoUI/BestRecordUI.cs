using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BestRecordUI : MonoBehaviour
{
	public TextMeshProUGUI recordText;

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

	void RefreshUI()
	{
		recordText.text = UnitCalculation.GetHeightText(GameManager.Instance.gameInfo.lastHeight, true);
	}
}
