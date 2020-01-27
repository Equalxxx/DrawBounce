using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGPGS : BasicUIButton
{
	public int btnIdx;
	public GameSettings gameSettings;

	protected override void PressedButton()
	{
		switch(btnIdx)
		{
			case 0:
				GooglePlayManager.Instance.SignIn();
				break;
			case 1:
				gameSettings.SaveInfoToServer();
				break;
			case 2:
				gameSettings.LoadGameInfo();
				break;
		}
	}
}
