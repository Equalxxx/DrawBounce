using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardUI : MonoBehaviour
{
	public int limitHeight = 100;
	public int addHeight = 100;
	public int addCoinValue;
	private RewardButton rewardButton;

	public void InitUI()
	{
		if (GameManager.Instance.player == null)
			return;

		Debug.Log("Reward Init");

		if (rewardButton == null)
			rewardButton = GetComponentInChildren<RewardButton>();

		int height = (int)GameManager.Instance.player.GetLastHeight();
		int startHeight = (int)GameManager.Instance.gameInfo.startHeight;

		if (height - startHeight >= limitHeight)
		{
			if (height >= 100)
			{
				addCoinValue = GetRewardValue(height);
			}
			else
			{
				addCoinValue = 100;
			}

			Show(true);
			rewardButton.coinText.text = addCoinValue.ToString();
		}
		else
		{
			addCoinValue = 0;
			Show(false);
		}
	}

	public void Show(bool show)
	{
		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);

		Debug.LogFormat("Show reward : {0}", show);
	}

	int GetRewardValue(int height)
	{
		int amount = height / addHeight;

		return (int)(amount * addHeight * (GameManager.Instance.player.addHeightCoinPer/100f));
	}
}
