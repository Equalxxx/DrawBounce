using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MysticLights;

[ExecuteInEditMode]
public class LocalizeSprite : BaseLocalizeObject {

    private Image myImage;
    public string spriteName;

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
        if(LocalizeManager.ShowLocalizeInfo)
            Debug.Log(string.Format("Show localizing prefab : {0} / {1}", LocalizeManager.LanguageType, spriteName));
        if (LocalizeManager.CheckLanguageType(languageType))
            return;

        if (myImage == null)
        {
            myImage = this.GetComponent<Image>();
        }

        Sprite spriteAsset = LocalizeManager.GetLocalizeAsset<Sprite>(spriteName, LocalizeManager.LocalizeAssetType.Sprite);
        if (spriteAsset == null)
            return;

        myImage.sprite = spriteAsset;
        myImage.SetNativeSize();

        languageType = LocalizeManager.LanguageType;
    }
}
