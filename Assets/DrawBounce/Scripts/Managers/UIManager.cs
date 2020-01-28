using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MysticLights;

public class UIManager : Singleton<UIManager>
{
    public UIGroup[] uiGroups;
    public UIGroup currentUIGroup;
	public RectTransform[] uiTransforms;
	public RectTransform canvasTrans;
	public float fadeDuration = 1f;

	public GameObject pauseUI;
	public GameObject practiceUI;
	public GameObject quitUI;
	public GameObject loadingUI;
	public TutorialUI tutorialUI;
	public ShowMessageUI showMessageUI;

	private void Start()
	{
		ShowPauseUI(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ShowQuitUI(true);
		}
	}

	public void ShowUIGroup(UIGroupType groupType)
    {
        if(currentUIGroup)
            currentUIGroup.ShowUIGroup(false);

        for (int i = 0; i < uiGroups.Length; i++)
        {
            if(groupType == uiGroups[i].groupType)
            {
                currentUIGroup = uiGroups[i];
			}
			else
			{
				uiGroups[i].ShowUIGroup(false);
			}
        }

		currentUIGroup.InitUI();
		currentUIGroup.ShowUIGroup(true);

        Debug.LogFormat("Change UI group : {0}", groupType);
    }

	public void ShowPauseUI(bool show)
	{
		if (pauseUI.activeSelf != show)
			pauseUI.SetActive(show);
	}

	public void ShowPracticeUI(bool show)
	{
		if(practiceUI.activeSelf != show)
			practiceUI.SetActive(show);
	}

	public void ShowLoadingUI(bool show)
	{
		if (loadingUI.activeSelf != show)
			loadingUI.SetActive(show);
	}

	public void ShowQuitUI(bool show)
	{
		if (quitUI.activeSelf != show)
			quitUI.SetActive(show);

		if (show)
			Time.timeScale = 0f;
		else
			Time.timeScale = 1f;
	}

	public void SetUIRects()
	{
		Debug.LogFormat("Test Size1 : {0}", Mathf.RoundToInt(100f * Screen.dpi / 160f / 2f));
		Debug.LogFormat("Test Size2 : {0}", AdmobManager.Instance.GetBannerHeight() / 2f - 50f);
		Debug.LogFormat("dpi : {0}", Screen.dpi); // 160f);
		Debug.LogFormat("adheight : {0}", adHeight());
		Debug.LogFormat("Test Size3 width : {0}, height : {1}", AdmobManager.Instance.GetBannerWidth(), AdmobManager.Instance.GetBannerHeight());
		Debug.LogFormat("screen width : {0}, height : {1}", Screen.width, Screen.height);
		Debug.LogFormat("ad width : {0}, height : {1}", AdmobManager.Instance.GetAdSizeWidth(), AdmobManager.Instance.GetAdSizeHeight());

		for (int i = 0; i < uiTransforms.Length; i++)
		{
			if(GameManager.Instance.isAds)
			{
				//uiTransforms[i].offsetMax = new Vector2(0f, -(50f * Screen.dpi / 160f));
				uiTransforms[i].offsetMax = new Vector2(0f, -(adHeight()));
			}
			else
			{
				uiTransforms[i].offsetMax = Vector2.zero;
			}
		}
	}
	public float adHeight()
	{
		float aspect = (Screen.height / canvasTrans.sizeDelta.y);
		float height = AdmobManager.Instance.GetBannerHeight() / aspect;

		return height;
	}
	//public float adHeight()
	//{
	//	float f = Screen.dpi / 160f;
	//	float dp = Screen.height / f;
	//	return (dp > 720f) ? 90f * f
	//		  : (dp > 400f) ? 50f * f
	//		  : 32f * f;
	//}
}
