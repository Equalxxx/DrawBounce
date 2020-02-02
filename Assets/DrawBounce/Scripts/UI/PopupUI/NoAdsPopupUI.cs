using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAdsPopupUI : ProtoPopupUI
{
	public override void InitPopupUI()
	{
		Time.timeScale = 0f;
	}

	public override void RefreshUI()
	{

	}

	public override void ClosePopupUI()
	{
		Time.timeScale = 1f;
		GameManager.Instance.CheckNoAds();
	}
}
