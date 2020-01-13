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

	public GameObject pauseUIObj;

	private void OnValidate()
	{
		uiGroups = GetComponentsInChildren<UIGroup>();
	}

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
                currentUIGroup = uiGroups[i];
                break;
            }
        }

        currentUIGroup.ShowUIGroup(true);
        Debug.LogFormat("Change UI group : {0}", groupType);
    }

	public void ShowPauseUI(bool show)
	{
		if (pauseUIObj.activeSelf != show)
		{
			pauseUIObj.SetActive(show);
		}
	}
}
