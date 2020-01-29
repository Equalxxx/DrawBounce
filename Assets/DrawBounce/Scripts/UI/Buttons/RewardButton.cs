using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MysticLights;

public class RewardButton : BasicUIButton
{
	public TextMeshProUGUI coinText;
	public int addCoinValue;
	private RewardUI rewardUI;

	protected override void InitButton()
	{
		if (rewardUI == null)
			rewardUI = GetComponentInParent<RewardUI>();

		addCoinValue = rewardUI.addCoinValue;
	}

	protected override void PressedButton()
	{
		if(GameManager.Instance.IsAddCoin(addCoinValue))
		{
			Debug.Log("Start reward ad success");
			if(GameManager.IsConnected && !GameManager.IsPracticeMode)
			{
				AdmobRewardAd.IsShowAd = true;
				AdmobManager.Instance.ShowAd(AdmobAdType.RewardVideo);
				StartCoroutine(WaitForAd());
			}
			else
			{
				Debug.LogFormat("Add Reward coin : {0}", addCoinValue);
				GameManager.Instance.AddCoin(addCoinValue);
				AddCoinEffect coinEffect = PoolManager.Instance.Spawn("AddCoinEffect", Camera.main.transform.position, Quaternion.identity).GetComponent<AddCoinEffect>();
				SoundManager.Instance.PlaySound2D("AddCoin");
				coinEffect.RefreshEffect(addCoinValue);
				rewardUI.Show(false);
			}
		}
		else
		{
			Debug.Log("Start reward ad failed");
		}
	}

	IEnumerator WaitForAd()
	{
		while(AdmobRewardAd.IsShowAd)
		{
			yield return null;
		}

		Debug.Log("Show ad is done");

		if (AdmobRewardAd.IsRewarded)
		{
			Debug.LogFormat("Add Reward coin : {0}",addCoinValue);
			GameManager.Instance.AddCoin(addCoinValue);
			AddCoinEffect coinEffect = PoolManager.Instance.Spawn("AddCoinEffect", Camera.main.transform.position, Quaternion.identity).GetComponent<AddCoinEffect>();
			SoundManager.Instance.PlaySound2D("AddCoin");
			coinEffect.RefreshEffect(addCoinValue);
			rewardUI.Show(false);

			AdmobRewardAd.IsRewarded = false;
		}
	}
}
