using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitingPopupUI : ProtoPopupUI
{
	public TextMeshProUGUI waitingText;
	public string savingText = "Saving...";
	public string loadingText = "Loading...";

	public override void InitPopupUI()
	{

	}

	public override void RefreshUI()
	{
		waitingText.text = loadingText;
	}

	public override void ClosePopupUI()
	{

	}
}
