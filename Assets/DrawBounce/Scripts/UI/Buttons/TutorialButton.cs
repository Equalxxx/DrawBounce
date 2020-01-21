using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialButton : BasicUIButton
{
	public bool isShow;

	private TutorialUI tutorialUI;

	protected override void InitButton()
	{
		if (tutorialUI == null)
			tutorialUI = UIManager.Instance.tutorialUI;
	}

	protected override void PressedButton()
	{
		tutorialUI.Show(isShow);
	}
}
