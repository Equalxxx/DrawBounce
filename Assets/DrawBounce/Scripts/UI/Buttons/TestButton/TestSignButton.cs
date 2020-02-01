using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSignButton : BasicUIButton
{
	public bool signIn;

	protected override void PressedButton()
	{
		if(signIn)
		{
			GooglePlayManager.Instance.SignIn();
			GameManager.Instance.gameSettings.LoadGameInfo();
		}
		else
		{
			GooglePlayManager.Instance.SignOut();
		}
	}
}
