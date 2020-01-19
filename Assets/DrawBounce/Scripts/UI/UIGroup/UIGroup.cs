using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIGroupType { Title, Game, Result, Shop }
public abstract class UIGroup : MonoBehaviour
{
    public UIGroupType groupType;
	private bool isShow;
    public CanvasGroup canvasGroup;

	public abstract void InitUI();

	public abstract void RefreshUI();

    public void ShowUIGroup(bool show)
    {
		isShow = show;

		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);

		canvasGroup.blocksRaycasts = false;

		if (show)
		{
			RefreshUI();
			StartCoroutine(FadeUIGroup());
		}

    }

    IEnumerator FadeUIGroup()
    {
        canvasGroup.alpha = 0f;

        float t = 0f;

        while (t < 1f)
        {
			if (!isShow)
				yield break;

            t += Time.deltaTime / UIManager.Instance.fadeDuration;
			
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);

            yield return null;
        }

        canvasGroup.blocksRaycasts = true;
    }
}
