using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialButton : BasicUIButton
{
	public bool isShow;

	private TutorialUI tutorialUI;
	private DOTweenAnimation myButtonAnim;

	protected override void InitButton()
	{
		if (tutorialUI == null)
			tutorialUI = UIManager.Instance.tutorialUI;
	}

	protected override void PressedButton()
	{
		tutorialUI.Show(isShow);
	}

	public void ShowButton(bool show)
	{
		if (!isShow)
			return;

		if (!myButtonAnim)
		{
			myButtonAnim = GetComponent<DOTweenAnimation>();
			if (!myButtonAnim)
				return;
		}

		if (show)
		{
			myButtonAnim.DOPlayForward();
		}
		else
		{
			myButtonAnim.DOPlayBackwards();
		}
	}
}
