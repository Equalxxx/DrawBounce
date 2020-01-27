using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPageButton : BasicUIButton
{
	private TutorialUI tutorialUI;
	public int dir = 1;

	protected override void PressedButton()
	{
		if (tutorialUI == null)
			tutorialUI = UIManager.Instance.tutorialUI;

		tutorialUI.ChangeTutorialPage(dir);
	}
}
