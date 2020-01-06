using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIGroupType { Title, Game, Result }
public class UIGroup : MonoBehaviour
{
    public UIGroupType groupType;

    protected CanvasGroup canvasGroup;

    public float fadeDuration = 1f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        canvasGroup.blocksRaycasts = false;
    }

    public void ShowUIGroup(bool show)
    {
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
                t += Time.deltaTime / fadeDuration;

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
