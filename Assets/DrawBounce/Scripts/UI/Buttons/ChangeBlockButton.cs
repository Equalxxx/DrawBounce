using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MysticLights;

public class ChangeBlockButton : BasicUIButton
{
	public PlayableBlockType blockType;
	private Image myImage;

	public Sprite onSprite;
	public Sprite offSprite;
	public float limitHeight;
	public GameObject lockObj;

	protected override void OnEnable()
	{
		base.OnEnable();

		if (myImage == null)
			myImage = GetComponent<Image>();

		RefreshUI(GameManager.Instance.curBlockType);
		GameManager.SetPlayableBlockAction += RefreshUI;
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		GameManager.SetPlayableBlockAction -= RefreshUI;
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
		GameManager.Instance.SetPlayableBlockType(blockType);
		GameManager.Instance.gameSettings.SaveDeviceOptions();
		SoundManager.Instance.PlaySound2D("Click", 0.5f);
	}

	void RefreshUI(PlayableBlockType pbType)
	{
		if(blockType == pbType)
		{
			myImage.sprite = onSprite;
		}
		else
		{
			myImage.sprite = offSprite;
		}
	}
}
