using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MysticLights
{
    public class PopupUIManager : Singleton_Prefab<PopupUIManager>
    {
        public ResourceManager.LinkType resLinkType = ResourceManager.LinkType.Resources;

        public ProtoPopupUI curPopupUI;
        public GameObject popupModal;
        public Transform popupParent;

        public List<ProtoPopupUI> popupUIList = new List<ProtoPopupUI>();

        //PopupUI Data
        private string tablePath = "datatables";
        private PopupUITable popupUITable;

        private void Awake()
        {
            popupUITable = ResourceManager.LoadAsset<PopupUITable>(tablePath, "PopupUITable", resLinkType);

            ShowModal(false);
        }

        public void ShowPopupUI(string popupName)
        {
            if (curPopupUI != null)
                return;

            ProtoPopupUI popupUI = GetPopupUI(popupName);
            if (popupUI == null)
            {
                return;
            }

            curPopupUI = popupUI;

            popupUI.InitPopupUI();
            popupUI.ShowPopupUI(true);
            ShowModal(true);
        }

        public void ClosePopupUI()
        {
            if (curPopupUI == null)
                return;

			curPopupUI.ClosePopupUI();
            curPopupUI.ShowPopupUI(false);
            curPopupUI = null;
            ShowModal(false);
        }

        ProtoPopupUI GetPopupUI(string tag)
        {
            ProtoPopupUI popupUI = popupUIList.Find(x => string.Equals(x.name, tag));
            if (popupUI == null)
            {
                //Resources 경로 설정
                PopupUITable.PopupUIInfo popupUIInfo = popupUITable.GetPopupUIInfo(tag);
                if (popupUIInfo == null)
                {
                    Debug.LogError("Not found PopupUIInfo : " + tag);
                    return null;
                }

                //string resourcePath = string.Format("{0}{1}", popupUIInfo.path, popupUIInfo.tag);

                GameObject newObj = ResourceManager.LoadAsset<GameObject>(popupUIInfo.path, popupUIInfo.tag, resLinkType);
                if (newObj == null)
                {
                    Debug.LogError("Not found popup ui : " + tag);
                    return null;
                }

                newObj = Instantiate(newObj);

                newObj.name = tag;
				RectTransform rectTransform = newObj.GetComponent<RectTransform>();
				rectTransform.SetParent(popupParent);
				rectTransform.localScale = Vector3.one;
				rectTransform.offsetMin = Vector2.zero;
				rectTransform.offsetMax = Vector2.zero;

				popupUI = newObj.GetComponent<ProtoPopupUI>();
                popupUIList.Add(popupUI);
            }

            return popupUI;
        }

        public void ShowModal(bool show)
        {
            popupModal.SetActive(show);
        }
    }
}