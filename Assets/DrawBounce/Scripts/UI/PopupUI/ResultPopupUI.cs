using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultPopupUI : ProtoPopupUI
{
	public Text descText;

	public override void InitPopupUI()
	{

	}

	public override void RefreshUI()
	{

	}

	public override void ClosePopupUI()
	{

	}

	public void SetDescription(string desc)
	{
		descText.text = desc;
	}
}
