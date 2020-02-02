using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPageButton : BasicUIButton
{
	private TutorialPopupUI tutorialUI;
	public int dir = 1;

	protected override void InitButton()
	{
		if (tutorialUI == null)
			tutorialUI = GetComponentInParent<TutorialPopupUI>();
	}

	protected override void PressedButton()
	{
		tutorialUI.ChangeTutorialPage(dir);
	}
}
