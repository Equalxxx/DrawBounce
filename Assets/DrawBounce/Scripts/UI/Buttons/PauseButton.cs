using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : BasicUIButton
{
	public bool pauseSwitch;

	protected override void InitButton() { }

	protected override void PressedButton()
	{
		GameManager.Instance.SetPause(pauseSwitch);
	}
}
