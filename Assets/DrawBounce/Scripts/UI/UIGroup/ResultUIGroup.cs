using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResultUIGroup : UIGroup
{
	public TextMeshProUGUI recordText;
	public TextMeshProUGUI maxMeterText;
	public int limitHeight = 100;

	private void OnValidate()
	{
		groupType = UIGroupType.Result;
	}

	private void OnEnable()
	{
		GameManager.GameOverAction += RefreshUI;
	}

	private void OnDisable()
	{
		GameManager.GameOverAction -= RefreshUI;
	}

	public override void InitUI()
	{
		int height = (int)GameManager.Instance.player.GetLastHeight();
		int startHeight = (int)GameManager.Instance.gameInfo.startHeight;

		if (height - startHeight >= limitHeight)
		{
			GameManager.Instance.StartCoroutine(ShowAd());
		}
	}

	public override void RefreshUI()
	{
		recordText.text = UnitCalculation.GetHeightText(GameManager.Instance.gameInfo.lastHeight, true);
		maxMeterText.text = UnitCalculation.GetHeightText(GameManager.Instance.player.GetLastHeight(), true);
	}

	IEnumerator ShowAd()
	{
		yield return new WaitForSeconds(0.5f);

		AdmobManager.Instance.ShowAd(AdmobAdType.Interstitial);
	}
}
