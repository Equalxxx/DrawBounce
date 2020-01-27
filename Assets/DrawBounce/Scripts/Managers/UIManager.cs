using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MysticLights;

public class UIManager : Singleton<UIManager>
{
    public UIGroup[] uiGroups;
    public UIGroup currentUIGroup;

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
}
