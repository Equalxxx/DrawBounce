using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleUIGroup : UIGroup
{
	private void OnValidate()
	{
		groupType = UIGroupType.Title;
	}

	protected override void InitUI()
	{

	}

	public override void RefreshUI()
	{

	}
}
