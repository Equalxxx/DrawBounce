using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MuteButton : BasicUIButton
{
	private Image myImage;
	private DOTweenAnimation myButtonAnim;

	public float animDuration = 1f;

	public Sprite onSoundSprite;
	public Sprite offSoundSprite;

	protected override void InitButton()
	{
		RefreshUI();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		GameManager.SoundMuteAction += RefreshUI;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.SoundMuteAction -= RefreshUI;
	}

	protected override void PressedButton()
	{
		GameManager.Instance.SetSoundMute(!GameManager.Instance.isSoundMute);
	}

	void RefreshUI()
	{
		if(!myImage)
			myImage = GetComponent<Image>();

		bool mute = GameManager.Instance.isSoundMute;

		if (mute)
		{
			myImage.sprite = offSoundSprite;
		}
		else
		{
			myImage.sprite = onSoundSprite;
		}
	}

	public void ShowButton(bool show)
	{
		if (!myButtonAnim)
		{
			myButtonAnim = GetComponent<DOTweenAnimation>();
			if (!myButtonAnim)
				return;

			myButtonAnim.duration = animDuration;
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
