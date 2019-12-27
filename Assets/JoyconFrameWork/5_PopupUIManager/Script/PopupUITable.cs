using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupUITable : ScriptableObject
{
    [System.Serializable]
    public class PopupUIInfo
    {
        public int index;
        public string path;
        public string tag;
    }

    public List<PopupUIInfo> popupUIInfoList = new List<PopupUIInfo>();

    public PopupUIInfo GetPopupUIInfo(string tag)
    {
        PopupUIInfo popupInfo = popupUIInfoList.Find(x => x.tag.Equals(tag));

        return popupInfo;
    }
}
