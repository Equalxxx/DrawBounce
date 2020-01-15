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

	public CanvasGroup pauseUI;

	private void Awake()
	{
		ShowPauseUI(false);
	}

	public void ShowUIGroup(UIGroupType groupType)
    {
        if(currentUIGroup)
            currentUIGroup.ShowUIGroup(false);

        for (int i = 0; i < uiGroups.Length; i++)
        {
            if(groupType == uiGroups[i].groupType)
            {
				uiGroups[i].ShowUIGroup(true);
                currentUIGroup = uiGroups[i];
			}
			else
			{
				uiGroups[i].ShowUIGroup(false);
			}
        }

        currentUIGroup.ShowUIGroup(true);
        Debug.LogFormat("Change UI group : {0}", groupType);
    }

	public void ShowPauseUI(bool show)
	{
		if(show)
		{
			pauseUI.alpha = 1f;
			pauseUI.blocksRaycasts = true;
		}
		else
		{
			pauseUI.alpha = 0f;
			pauseUI.blocksRaycasts = false;
		}
	}
}
