using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MysticLights;

public class TutorialUI : MonoBehaviour
{
	public int pageIndex;

	public GameObject[] tutorialPages;
	public Image[] pagePoints;
	public Sprite onSprite;
	public Sprite offSprite;

	public void Show(bool show)
	{
		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);

		if (show)
		{
			Time.timeScale = 0f;
			pageIndex = 0;
			RefreshUI();
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void ChangeTutorialPage(int dir)
	{
		pageIndex += dir;

		if (pageIndex < 0)
			pageIndex = 0;

		if (pageIndex >= tutorialPages.Length)
			pageIndex = tutorialPages.Length - 1;

		RefreshUI();
	}

	void RefreshUI()
	{
		for(int i = 0; i < pagePoints.Length; i++)
		{
			if (i == pageIndex)
				pagePoints[i].sprite = onSprite;
			else
				pagePoints[i].sprite = offSprite;
		}

		for(int i = 0; i < tutorialPages.Length; i++)
		{
			if (i == pageIndex)
				tutorialPages[i].SetActive(true);
			else
				tutorialPages[i].SetActive(false);
		}
	}
}
