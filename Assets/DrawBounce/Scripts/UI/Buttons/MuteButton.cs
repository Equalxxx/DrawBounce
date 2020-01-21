using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MuteButton : BasicUIButton
{
	private Image myImage;
	private DOTweenAnimation myButtonAnim;

	public SoundType soundType;
	public bool isMute;

	public Color onColor;
	public Color offColor;

	protected override void OnEnable()
	{
		base.OnEnable();
		GameManager.GameSettingAction += SetMute;
		GameManager.SoundMuteAction += SetMute;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.GameSettingAction -= SetMute;
		GameManager.SoundMuteAction -= SetMute;
	}

	protected override void PressedButton()
	{
		isMute = !isMute;

		GameManager.Instance.SetSoundMute(soundType, isMute);
		GameManager.Instance.gameSettings.SaveDeviceOptions();

		RefreshUI();
	}

	void SetMute()
	{
		switch(soundType)
		{
			case SoundType.BGM:
				isMute = GameManager.Instance.isMuteBGM;
				break;
			case SoundType.SE:
				isMute = GameManager.Instance.isMuteSE;
				break;
		}

		RefreshUI();
	}

	void RefreshUI()
	{
		if(!myImage)
			myImage = GetComponent<Image>();

		myImage.color = isMute ? offColor : onColor;
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
