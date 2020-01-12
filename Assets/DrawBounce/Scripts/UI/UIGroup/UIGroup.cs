using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIGroupType { Title, Game, Result, Shop }
public class UIGroup : MonoBehaviour
{
    public UIGroupType groupType;
	private bool isShow;
    protected CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowUIGroup(bool show)
    {
		isShow = show;
		StartCoroutine(FadeUIGroup(show));
    }

    IEnumerator FadeUIGroup(bool show)
    {
        float t = 0f;

        if (show)
        {
            canvasGroup.alpha = 0f;

            while (t < 1f)
            {
				if (!isShow)
					yield break;

                t += Time.deltaTime / UIManager.Instance.fadeDuration;

                if (show)
                {
                    canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
                }
                else
                {
                    canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
                }

                yield return null;
            }

            canvasGroup.blocksRaycasts = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
    }
}
