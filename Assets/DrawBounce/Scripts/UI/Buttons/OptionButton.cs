using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OptionButton : BasicUIButton
{
	public bool isShow;
	private DOTweenAnimation buttonAnim;

	public MuteButton muteBGMButton;
	public MuteButton muteSEButton;
	public VibeButton vibeButton;
	public TutorialButton tutorialButton;

	protected override void OnEnable()
	{
		base.OnEnable();

		isShow = false;
		buttonAnim.DOPlayBackwards();
		muteBGMButton.ShowButton(false);
		muteSEButton.ShowButton(false);
		vibeButton.ShowButton(false);
		tutorialButton.ShowButton(false);
	}

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
			vibeButton.ShowButton(true);
			tutorialButton.ShowButton(true);
		}
		else
		{
			buttonAnim.DOPlayBackwards();
			muteBGMButton.ShowButton(false);
			muteSEButton.ShowButton(false);
			vibeButton.ShowButton(false);
			tutorialButton.ShowButton(false);
		}
	}
}
