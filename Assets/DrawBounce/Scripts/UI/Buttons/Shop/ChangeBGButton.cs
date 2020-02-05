using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MysticLights;

public class ChangeBGButton : BasicUIButton
{
	private Image myImage;

	public int bgIndex;
	public float limitHeight;

	public Sprite onSprite;
	public Sprite offSprite;
	public GameObject lockObj;

	protected override void OnEnable()
	{
		base.OnEnable();

		if (myImage == null)
			myImage = GetComponent<Image>();

		RefreshUI(GameManager.Instance.bgControl.bgIndex);
		GameManager.SetBGAction += RefreshUI;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.SetBGAction -= RefreshUI;
	}

	protected override void InitButton()
	{
		if (GameManager.Instance.gameInfo.lastHeight >= limitHeight)
		{
			myButton.interactable = true;
			lockObj.SetActive(false);
		}
		else
		{
			myButton.interactable = false;
			lockObj.SetActive(true);
		}
	}

	protected override void PressedButton()
	{
		GameManager.Instance.SetBGColor(bgIndex);
		GameManager.Instance.gameSettings.SaveDeviceOptions();
		SoundManager.Instance.PlaySound2D("Click", 0.5f);
	}

	void RefreshUI(int bgIdx)
	{
		if (bgIndex == bgIdx)
		{
			myImage.sprite = onSprite;
		}
		else
		{
			myImage.sprite = offSprite;
		}
	}
}
