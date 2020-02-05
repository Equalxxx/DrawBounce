using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MysticLights;

public class RewardButton : BasicUIButton
{
	public TextMeshProUGUI coinText;
	public int addCoinValue;
	private RewardUI rewardUI;

	public Image adsImage;
	public Image boxImage;

	protected override void InitButton()
	{
		if (rewardUI == null)
			rewardUI = GetComponentInParent<RewardUI>();

		if (GameManager.IsNoAds)
		{
			adsImage.enabled = false;
			boxImage.enabled = true;
		}
		else
		{
			adsImage.enabled = true;
			boxImage.enabled = false;
		}

		addCoinValue = rewardUI.addCoinValue;
	}

	protected override void PressedButton()
	{
		if(GameManager.Instance.IsAddCoin(addCoinValue))
		{
			Debug.Log("Start reward ad success");
			if (GameManager.IsConnected && !GameManager.IsPracticeMode)
			{
				if (!GameManager.IsNoAds)
				{
					AdmobManager.IsShowAd = true;
					AdmobManager.Instance.ShowAd(AdmobAdType.RewardVideo);
					StartCoroutine(WaitForAd());
				}
				else
				{
					AddCoinProcess();

					GameManager.Instance.SaveGame();
				}
			}
			else
			{
				AddCoinProcess();
			}
		}
		else
		{
			Debug.Log("Start reward ad failed");
		}
	}

	IEnumerator WaitForAd()
	{
		while(AdmobManager.IsShowAd)
		{
			yield return null;
		}

		Debug.Log("Show ad is done");

		if (AdmobManager.IsRewarded)
		{
			AddCoinProcess();

			AdmobManager.IsRewarded = false;

			GameManager.Instance.SaveGame();
		}
	}

	void AddCoinProcess()
	{
		Debug.LogFormat("Add Reward coin : {0}", addCoinValue);
		GameManager.Instance.AddCoin(addCoinValue);
		AddCoinEffect coinEffect = PoolManager.Instance.Spawn("AddCoinEffect", Camera.main.transform.position, Quaternion.identity).GetComponent<AddCoinEffect>();
		SoundManager.Instance.PlaySound2D("AddCoin");
		coinEffect.RefreshEffect(addCoinValue);
		rewardUI.Show(false);
	}
}
