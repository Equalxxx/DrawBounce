using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleUIGroup : UIGroup
{
	public TextMeshProUGUI recordText;

	private void OnValidate()
	{
		groupType = UIGroupType.Title;
	}

	public override void InitUI()
	{

	}

	public override void RefreshUI()
	{
		recordText.text = UnitCalculation.GetHeightText(GameManager.Instance.gameInfo.lastHeight, true);
	}
}
