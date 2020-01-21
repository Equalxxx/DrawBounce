using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : BasicUIButton
{
	public bool isQuit;

	protected override void PressedButton()
	{
		if (isQuit)
			Application.Quit();
		else
			UIManager.Instance.ShowQuitUI(false);
	}
}
