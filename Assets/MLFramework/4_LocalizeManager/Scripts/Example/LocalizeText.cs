using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MysticLights;

[ExecuteInEditMode]
public class LocalizeText : BaseLocalizeObject
{
    private Text strText;
    public int stringIndex;

    private void Start()
    {
        ShowLocalize();
    }

    private void OnValidate()
    {
        ShowLocalize();
    }

    public override void ShowLocalize()
    {
        if (strText == null)
        {
            strText = this.GetComponent<Text>();
        }
        
        string strValue = LocalizeManager.GetStr(stringIndex);
        strText.font = LocalizeManager.GetFont(stringIndex);

        if (LocalizeManager.ShowLocalizeInfo)
        {
            strText.horizontalOverflow = HorizontalWrapMode.Overflow;
            strText.text = string.Format("{0} ({1})", strValue, stringIndex);

            Debug.Log(string.Format("Show localizing text : {0} / {1} ({2})", LocalizeManager.LanguageType, strValue, stringIndex));
        }
        else
        {
            strText.horizontalOverflow = HorizontalWrapMode.Wrap;
            strText.text = strValue;
        }

        languageType = LocalizeManager.LanguageType;
    }
}
