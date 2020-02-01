using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleUIGroup : UIGroup
{
	public GameObject noAdsBtn;

	private void OnValidate()
	{
		groupType = UIGroupType.Title;
	}

	public override void InitUI()
	{
		if (GameManager.IsConnected && GameManager.IsNoAds)
			noAdsBtn.SetActive(true);
		else
			noAdsBtn.SetActive(false);
	}

	public override void RefreshUI()
	{
	}
}
