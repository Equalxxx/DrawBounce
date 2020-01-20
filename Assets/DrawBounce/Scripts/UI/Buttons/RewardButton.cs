using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardButton : BasicUIButton
{
	public TextMeshProUGUI coinText;
	public int addCoinValue;
	public int limitHeight = 100;
	public GameObject btnObject;

	protected override void OnEnable()
	{
		base.OnEnable();

		AdmobManager.AdRewardVideoAction += AddReward;
	}

	protected override void OnDisable()
	{
		base.OnDisable();

		AdmobManager.AdRewardVideoAction -= AddReward;
	}

	protected override void InitButton()
	{
		Debug.Log("Reward Button Init");
		if (GameManager.Instance.player == null)
			return;

		int height = (int)GameManager.Instance.player.GetLastHeight();
		int startHeight = (int)GameManager.Instance.gameInfo.startHeight;

		if(height - startHeight >= limitHeight)
		{
			if(height >= 1000)
			{
				addCoinValue = 1000;
			}
			else if(height >= 500)
			{
				addCoinValue = 500;
			}
			else if (height >= 300)
			{
				addCoinValue = 300;
			}
			else
			{
				addCoinValue = 100;
			}

			Show(true);
			coinText.text = addCoinValue.ToString();
		}
		else
		{
			addCoinValue = 0;
			Show(false);
		}
	}

	protected override void PressedButton()
	{
		if(GameManager.Instance.IsAddCoin(addCoinValue))
		{
			Debug.Log("Reward add coin success");

			AdmobManager.Instance.ShowAd(AdmobAdType.RewardVideo);
		}
		else
		{
			Debug.Log("Reward add coin failed");
		}
	}

	void Show(bool show)
	{
		myButton.interactable = show;

		if (btnObject.activeSelf != show)
			btnObject.SetActive(show);

		Debug.LogFormat("Show reward button : {0}", show);
	}

	void AddReward()
	{
		GameManager.Instance.AddCoin(addCoinValue);
		Show(false);
	}
}
