using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLFramework;

public abstract class BaseLocalizeObject : MonoBehaviour {

    protected LocalizeManager.LocalizeLanguageType languageType;

    private void OnEnable()
    {
        LocalizeManager.OnShowLocalizeInfo += ShowLocalize;
        LocalizeManager.OnChangeLanguage += ShowLocalize;
    }

    private void OnDisable()
    {
        LocalizeManager.OnShowLocalizeInfo -= ShowLocalize;
        LocalizeManager.OnChangeLanguage -= ShowLocalize;
    }

    public abstract void ShowLocalize();
}
