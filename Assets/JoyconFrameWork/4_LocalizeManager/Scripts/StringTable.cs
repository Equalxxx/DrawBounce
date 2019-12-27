using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringTable : ScriptableObject {
    [System.Serializable]
    public class StringInfo
    {
        public int index;
        public string stringData;
        public string fontName;
    }

    public List<StringInfo> stringDataList = new List<StringInfo>();

    public string GetStringData(int index)
    {
        StringInfo stringInfo = stringDataList.Find(x => x.index.Equals(index));

        if(stringInfo == null)
        {
            Debug.LogError("Not found StringData : " + index);
            return "";
        }

        return stringInfo.stringData;
    }

    public string GetFontData(int index)
    {
        StringInfo stringInfo = stringDataList.Find(x => x.index.Equals(index));

        if (stringInfo == null)
        {
            Debug.LogError("Not found StringData : " + index);
            return "";
        }

        return stringInfo.fontName;
    }
}
