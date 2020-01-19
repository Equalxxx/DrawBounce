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
	public GameObject practiceObj;
	public NotConnectedUI notConnectedUI;

	private void Awake()
	{
		ShowPauseUI(false);
	}

	private void OnEnable()
	{
		GooglePlayManager.SignInAction += ShowPracticeUI;
	}

	private void OnDisable()
	{
		GooglePlayManager.SignInAction -= ShowPracticeUI;
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

	void ShowPracticeUI(bool success)
	{
		if(practiceObj.activeSelf == success)
			practiceObj.SetActive(!success);
	}
}
