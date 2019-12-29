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
    }

    public void ShowUIGroup(bool show)
    {
        StartCoroutine(FadeUIGroup(show));
    }

    IEnumerator FadeUIGroup(bool show)
    {
        if (show)
        {
            canvasGroup.alpha = 0f;
        }
        else
        {
            canvasGroup.alpha = 1f;
        }

        float t = 0f;

        while(true)
        {
            t += Time.deltaTime / fadeDuration;

            if(show)
            {
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            }
            else
            {
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t);
            }

            yield return null;
        }
    }
}
