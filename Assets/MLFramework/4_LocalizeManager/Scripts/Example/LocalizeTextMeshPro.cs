using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;
using TMPro;

[ExecuteInEditMode]
public class LocalizeTextMeshPro : BaseLocalizeObject
{
    private TextMeshProUGUI strText;
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
            strText = this.GetComponent<TextMeshProUGUI>();
        }
        
        string strValue = LocalizeManager.GetStr(stringIndex);
		strText.font = LocalizeManager.GetTMPFont(stringIndex);

		if (LocalizeManager.ShowLocalizeInfo)
        {
			strText.enableWordWrapping = false;
            strText.text = string.Format("{0} ({1})", strValue, stringIndex);

            Debug.Log(string.Format("Show localizing text : {0} / {1} ({2})", LocalizeManager.LanguageType, strValue, stringIndex));
        }
        else
        {
			strText.enableWordWrapping = true;
			strText.text = strValue;
        }

        languageType = LocalizeManager.LanguageType;
    }
}
