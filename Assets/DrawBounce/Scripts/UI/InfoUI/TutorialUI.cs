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
			GameManager.Instance.player.Show(false);
			RefreshUI();
		}
		else
		{
			GameManager.Instance.player.Show(true);
		}
	}

	public void RefreshUI()
	{

	}
}
