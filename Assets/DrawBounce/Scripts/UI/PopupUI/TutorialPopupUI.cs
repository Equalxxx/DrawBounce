using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLFramework;

public class TutorialPopupUI : ProtoPopupUI
{
	public int pageIndex;

	public GameObject[] tutorialPages;
	public Image[] pagePoints;
	public Sprite onSprite;
	public Sprite offSprite;

	public void ChangeTutorialPage(int dir)
	{
		pageIndex += dir;

		if (pageIndex < 0)
			pageIndex = 0;

		if (pageIndex >= tutorialPages.Length)
			pageIndex = tutorialPages.Length - 1;

		RefreshUI();
	}

	public override void InitPopupUI()
	{
		popupUIType = PopupUIType.Tutorial;
		
		pageIndex = 0;

		GameManager.Instance.curPlayableBlock.Show(false);
	}

	public override void RefreshUI()
	{
		for (int i = 0; i < pagePoints.Length; i++)
		{
			if (i == pageIndex)
				pagePoints[i].sprite = onSprite;
			else
				pagePoints[i].sprite = offSprite;
		}

		for (int i = 0; i < tutorialPages.Length; i++)
		{
			if (i == pageIndex)
				tutorialPages[i].SetActive(true);
			else
				tutorialPages[i].SetActive(false);
		}
	}

	public override void ClosePopupUI()
	{
		GameManager.Instance.curPlayableBlock.Show(true);
	}
}
