using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class VibeButton : BasicUIButton
{
	private Image myImage;
	private DOTweenAnimation myButtonAnim;

	public bool isVibe;

	public Color onColor;
	public Color offColor;

	protected override void OnEnable()
	{
		base.OnEnable();
		GameSettings.GameSettingAction += SetVibe;
		GameManager.ViberateAction += SetVibe;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameSettings.GameSettingAction -= SetVibe;
		GameManager.ViberateAction -= SetVibe;
	}

	protected override void PressedButton()
	{
		isVibe = !isVibe;
		GameManager.Instance.SetViberate(isVibe);
		GameManager.Instance.gameSettings.SaveDeviceOptions();
		RefreshUI();
	}

	void SetVibe()
	{
		isVibe = GameManager.Instance.deviceSettings.viberate;

		RefreshUI();
	}

	void RefreshUI()
	{
		if (!myImage)
			myImage = GetComponent<Image>();

		myImage.color = isVibe ? onColor : offColor;
	}

	public void ShowButton(bool show)
	{
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

		RefreshUI();
	}
}
