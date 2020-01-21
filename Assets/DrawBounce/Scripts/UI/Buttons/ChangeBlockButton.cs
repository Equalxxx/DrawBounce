using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeBlockButton : BasicUIButton
{
	public PlayableBlockType blockType;
	private Image myImage;

	public Sprite onSprite;
	public Sprite offSprite;

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

	protected override void PressedButton()
	{
		GameManager.Instance.SetPlayableBlockType(blockType);
		GameManager.Instance.gameSettings.SaveDeviceOptions();
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
