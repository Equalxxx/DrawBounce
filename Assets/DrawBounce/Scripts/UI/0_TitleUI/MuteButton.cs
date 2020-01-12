using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MuteButton : MonoBehaviour
{
	private Button button;
	private Image myImage;

	private bool isMute;
	private DOTweenAnimation buttonAnim;

	public Sprite onSoundSprite;
	public Sprite offSoundSprite;

	public float animDuration = 1f;

	private void Awake()
	{
		myImage = GetComponent<Image>();
		button = GetComponent<Button>();
		buttonAnim = GetComponent<DOTweenAnimation>();

		buttonAnim.duration = animDuration;
	}

	private void OnEnable()
	{
		button.onClick.AddListener(PressedButton);
	}

	private void OnDisable()
	{
		button.onClick.RemoveListener(PressedButton);
	}

	void PressedButton()
	{
		isMute = !isMute;

		if (isMute)
		{
			myImage.sprite = offSoundSprite;
		}
		else
		{
			myImage.sprite = onSoundSprite;
		}

		GameManager.Instance.gameSettings.SetSoundMute(isMute);
	}

	public void ShowButton(bool show)
	{
		if (show)
		{
			buttonAnim.DOPlayForward();
		}
		else
		{
			buttonAnim.DOPlayBackwards();
		}
	}
}
