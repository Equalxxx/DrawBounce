using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleUIGroup : UIGroup
{
	private void OnValidate()
	{
		groupType = UIGroupType.Title;
	}

	public override void InitUI()
	{

	}

	public override void RefreshUI()
	{
	}
}
