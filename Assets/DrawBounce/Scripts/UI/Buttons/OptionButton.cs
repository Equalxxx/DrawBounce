﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OptionButton : BasicUIButton
{
	private bool isShow;
	private DOTweenAnimation buttonAnim;

	public MuteButton muteBGMButton;
	public MuteButton muteSEButton;

	protected override void InitButton()
	{
		buttonAnim = GetComponent<DOTweenAnimation>();
	}

	protected override void PressedButton()
	{
		isShow = !isShow;

		if (isShow)
		{
			buttonAnim.DOPlayForward();
			muteBGMButton.ShowButton(true);
			muteSEButton.ShowButton(true);
		}
		else
		{
			buttonAnim.DOPlayBackwards();
			muteBGMButton.ShowButton(false);
			muteSEButton.ShowButton(false);
		}
	}
}
