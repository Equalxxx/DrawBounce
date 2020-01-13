using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MuteButton : BasicUIButton
{
	private Image myImage;
	private DOTweenAnimation myButtonAnim;

	public bool isMute;
	public float animDuration = 1f;

	public Sprite onSoundSprite;
	public Sprite offSoundSprite;

	protected override void InitButton()
	{
		myImage = GetComponent<Image>();
		myButtonAnim = GetComponent<DOTweenAnimation>();

		myButtonAnim.duration = animDuration;

		isMute = GameManager.Instance.isSoundMute;
	}

	protected override void PressedButton()
	{
		isMute = !isMute;

		ChangeMuteSprite();

		GameManager.Instance.SetSoundMute(isMute);
	}

	void ChangeMuteSprite()
	{
		if (isMute)
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
		isMute = GameManager.Instance.isSoundMute;
		ChangeMuteSprite();

		if (show)
		{
			myButtonAnim.DOPlayForward();
		}
		else
		{
			myButtonAnim.DOPlayBackwards();
		}
	}
}
