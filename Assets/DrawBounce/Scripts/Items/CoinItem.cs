﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public class CoinItem : BasicItem
{
	private bool follow;
	private Vector3 originPosition;
	private Transform myTransform;
	private Transform targetTransform;
	private float followSpeed = 7f;
	private int addHeight = 100;

	public override void InitItem()
	{
		if (myTransform == null)
		{
			myTransform = transform;
			originPosition = myTransform.position;
		}
		
		targetTransform = GameManager.Instance.player.transform;

		myTransform.position = originPosition;

		follow = false;

		Show(true);
	}

	private void Update()
	{
		if (!follow)
			return;

		myTransform.position = Vector3.Lerp(myTransform.position, targetTransform.position, Time.deltaTime * followSpeed);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.CompareTag("ItemRange"))
		{
			follow = true;
		}

		if (collision.CompareTag("Player"))
		{
			int addCoinValue = GetCoinValue();
			if (GameManager.Instance.IsAddCoin(addCoinValue))
			{
				GameManager.Instance.AddCoin(addCoinValue);
				AddCoinEffect coinEffect = PoolManager.Instance.Spawn("AddCoinEffect", myTransform.position, Quaternion.identity).GetComponent<AddCoinEffect>();
				SoundManager.Instance.PlaySound2D("AddCoin");

				coinEffect.RefreshEffect(addCoinValue);
			}

			Show(false);
		}
	}

	public void Show(bool show)
	{
		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);
	}

	int GetCoinValue()
	{
		int height = (int)GameManager.Instance.player.GetLastHeight();
		int amount = height / addHeight + 1;

		return (int)(amount * (GameManager.Instance.player.addHeightCoinPer / 100f));
	}
}