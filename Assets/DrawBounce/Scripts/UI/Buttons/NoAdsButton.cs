using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAdsButton : BasicUIButton
{
	public bool show;

	protected override void OnEnable()
	{
		base.OnEnable();
		GameManager.CheckNoAdsAction += RefreshUI;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.CheckNoAdsAction -= RefreshUI;
	}

	protected override void InitButton()
	{
		base.InitButton();
	}

	protected override void PressedButton()
	{
		UIManager.Instance.ShowNoAdsUI(show);
	}

	void RefreshUI()
	{
		if (show && GameManager.IsNoAds)
			gameObject.SetActive(false);
	}
}
