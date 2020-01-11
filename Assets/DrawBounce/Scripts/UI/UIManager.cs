using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public class UIManager : Singleton<UIManager>
{
    public UIGroup[] uiGroups;
    public UIGroup currentUIGroup;
    
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

	public string GetMeterText(float height)
	{
		return string.Format("{0:f1}M", height);
	}
}
