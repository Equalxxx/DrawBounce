using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitButton : BasicUIButton
{
	protected override void PressedButton()
	{
		Application.Quit();
	}
}
