﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MysticLights
{
    public class LocalizeManager : Singleton<LocalizeManager>
    {
        public enum LocalizeLanguageType { KR, ENG }
        public enum LocalizeAssetType { Texture, Sprite, Prefab }

        private ResourceManager.LinkType resLinkType = ResourceManager.LinkType.Resources;
        public StringTable stringTable;

        public static LocalizeLanguageType LanguageType
        {
            get { return Instance.languageType; }
        }
        public LocalizeLanguageType languageType;
        private LocalizeLanguageType oldLanguageType;

        public static bool ShowLocalizeInfo
        {
            get { return Instance.showLocalizeInfo; }
        }
        public bool showLocalizeInfo;
        private bool oldShowLocalizeInfo;

        public static Action OnChangeLanguage;
        public static Action OnShowLocalizeInfo;

        private string dataTablePath = "DataTables";
        private string fontPath = "Fonts";
        private string prefabPath = "Prefabs";
        private string texturePath = "Textures";
        private string spritePath = "Sprites";

        private void OnValidate()
        {
            if (languageType != oldLanguageType)
            {
                oldLanguageType = languageType;
                stringTable = null;
                LoadStringTable();
                if (OnChangeLanguage != null)
                    OnChangeLanguage();
            }

            if (showLocalizeInfo != oldShowLocalizeInfo)
            {
                oldShowLocalizeInfo = showLocalizeInfo;
                if (OnShowLocalizeInfo != null)
                    OnShowLocalizeInfo();

                Debug.Log("Show string index : " + showLocalizeInfo);
            }
        }

        private void Awake()
        {
            if (Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            if (OnChangeLanguage != null)
                OnChangeLanguage();
        }

        private void Update()
        {
            //testcode
            if (Input.GetKeyDown(KeyCode.L))
            {
                languageType++;
                if (languageType > LocalizeLanguageType.ENG)
                    languageType = LocalizeLanguageType.KR;
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                showLocalizeInfo = !showLocalizeInfo;
            }

            if (languageType != oldLanguageType)
            {
                oldLanguageType = languageType;
                stringTable = null;
                LoadStringTable();
                if (OnChangeLanguage != null)
                    OnChangeLanguage();
            }

            if (showLocalizeInfo != oldShowLocalizeInfo)
            {
                oldShowLocalizeInfo = showLocalizeInfo;
                if (OnShowLocalizeInfo != null)
                    OnShowLocalizeInfo();

                Debug.Log("Show string index : " + showLocalizeInfo);
            }
        }

        private void OnDestroy()
        {
            ResourceManager.ReleaseAll();
        }

        public static string GetStr(int strIndex)
        {
            if (!Instance.LoadStringTable())
            {
                return "";
            }

            return Instance.stringTable.GetStringData(strIndex).Replace("\\n", "\n");
        }

        public static Font GetFont(int strIndex)
        {
            if (!Instance.LoadStringTable())
            {
                return null;
            }

            string fontName = Instance.stringTable.GetFontData(strIndex);
            if (fontName.Equals("Arial"))
            {
                return Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
            }

            Font font = ResourceManager.LoadAsset<Font>(Instance.fontPath, fontName, Instance.resLinkType);

            if (font == null)
            {
                font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
                Debug.LogError("Not found Font : " + fontName);
            }

            return font;
        }

        public static T GetLocalizeAsset<T>(string assetName, LocalizeAssetType assetType)
        {
            string assetPath = "";
            switch (assetType)
            {
                case LocalizeAssetType.Texture:
                    assetPath = string.Format("{0}/{1}", Instance.texturePath, LanguageType);
                    break;
                case LocalizeAssetType.Sprite:
                    assetPath = string.Format("{0}/{1}", Instance.spritePath, LanguageType);
                    break;
                case LocalizeAssetType.Prefab:
                    assetPath = string.Format("{0}/{1}", Instance.prefabPath, LanguageType);
                    break;
            }

            T localizeAsset = ResourceManager.LoadAsset<T>(assetPath, assetName, Instance.resLinkType);
            if (localizeAsset == null)
            {
                Debug.LogError(string.Format("Not found {0} : {1} / {2}", assetType, assetName, LanguageType));
            }

            return localizeAsset;
        }

        bool LoadStringTable()
        {
            if (stringTable != null)
                return true;

            stringTable = ResourceManager.LoadAsset<StringTable>(dataTablePath, "StringTable_" + languageType.ToString(), resLinkType);

            if (stringTable == null)
            {
                Debug.LogError("Not found StringTable : " + languageType);
                return false;
            }

            Debug.Log("Load Complete StringTable : " + languageType);
            return true;
        }

        public static bool CheckLanguageType(LocalizeLanguageType type)
        {
            return LanguageType == type ? true : false;
        }
    }
}