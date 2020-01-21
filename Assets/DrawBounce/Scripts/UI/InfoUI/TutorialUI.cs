using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public void Show(bool show)
	{
		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);

		if (show)
		{
			Time.timeScale = 0f;
			RefreshUI();
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void RefreshUI()
	{

	}
}
