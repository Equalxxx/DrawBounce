using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

[ExecuteInEditMode]
public class LocalizePrefab : BaseLocalizeObject
{
    public string prefabName;

    private void Start()
    {
        ShowLocalize();
    }

    private void OnValidate()
    {
        ShowLocalize();
    }

    IEnumerator Destroy(GameObject go)
    {
        yield return new WaitForEndOfFrame();
        DestroyImmediate(go);
    }

    public override void ShowLocalize()
    {
        if (LocalizeManager.ShowLocalizeInfo)
            Debug.Log(string.Format("Show localizing prefab : {0} / {1}", LocalizeManager.LanguageType, prefabName));
        if (LocalizeManager.CheckLanguageType(languageType))
            return;

        foreach (Transform child in transform)
        {
            StartCoroutine(Destroy(child.gameObject));
        }

        GameObject prefabAsset = LocalizeManager.GetLocalizeAsset<GameObject>(prefabName, LocalizeManager.LocalizeAssetType.Prefab);
        if (prefabAsset == null)
            return;

        GameObject newObj = Instantiate(prefabAsset, this.transform);
        newObj.name = prefabName;
        newObj.transform.localPosition = Vector3.zero;
        newObj.transform.localRotation = Quaternion.identity;
        newObj.transform.localScale = Vector3.one;

        languageType = LocalizeManager.LanguageType;
    }
}
