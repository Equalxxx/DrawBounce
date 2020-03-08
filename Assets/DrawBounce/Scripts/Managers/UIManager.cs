using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLFramework;

public enum WaitingPopupType { Saving, Loading }

public class UIManager : Singleton<UIManager>
{
    public UIGroup[] uiGroups;
    public UIGroup currentUIGroup;
	public RectTransform uiTransform;
	public RectTransform popupTransform;
	public RectTransform canvasTrans;
	public float fadeDuration = 1f;
	
	public ShowMessageUI showMessageUI;
	public GameObject noAdsButton;
	public GameObject offlineMode;
	public CoinInfoUI coinInfoUI;

	private void Awake()
	{
		ShowOfflineMode(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (PopupUIManager.Instance.curPopupUI == null)
				ShowPopup(PopupUIType.Quit, true);
			else
				PopupUIManager.Instance.ClosePopupUI();
		}
	}

	public void ShowUIGroup(UIGroupType groupType)
    {
        if(currentUIGroup)
            currentUIGroup.ShowUIGroup(false);

        for (int i = 0; i < uiGroups.Length; i++)
        {
            if(groupType == uiGroups[i].groupType)
            {
                currentUIGroup = uiGroups[i];
			}
			else
			{
				uiGroups[i].ShowUIGroup(false);
			}
        }

		currentUIGroup.InitUI();
		currentUIGroup.ShowUIGroup(true);

        Debug.LogFormat("Change UI group : {0}", groupType);
    }

	public void ShowPopup(PopupUIType popupType, bool show)
	{
		if(show)
		{
			switch(popupType)
			{
				case PopupUIType.Quit:
					PopupUIManager.Instance.ShowPopupUI("QuitPopupUI");
					break;
				case PopupUIType.Pause:
					PopupUIManager.Instance.ShowPopupUI("PausePopupUI");
					break;
				case PopupUIType.Tutorial:
					PopupUIManager.Instance.ShowPopupUI("TutorialPopupUI");
					break;
				case PopupUIType.NoAds:
					PopupUIManager.Instance.ShowPopupUI("NoAdsPopupUI");
					break;
				case PopupUIType.Option:
					PopupUIManager.Instance.ShowPopupUI("OptionPopupUI");
					break;
				case PopupUIType.SaveData:
					PopupUIManager.Instance.ShowPopupUI("SaveDataPopupUI");
					break;
				case PopupUIType.LoadData:
					PopupUIManager.Instance.ShowPopupUI("LoadDataPopupUI");
					break;
				case PopupUIType.Review:
					PopupUIManager.Instance.ShowPopupUI("ReviewPopupUI");
					break;
			}
		}
		else
		{
			if(PopupUIManager.Instance.curPopupUI && PopupUIManager.Instance.curPopupUI.popupUIType == popupType)
				PopupUIManager.Instance.ClosePopupUI();
		}
	}

	public void ShowLoadingUI(bool show)
	{
		PopupUIManager.Instance.ShowLoadingUI(show);
	}

	public void SetUIRects()
	{
		//if (!GameManager.IsNoAds && GameManager.IsConnected)
		//{
		//	uiTransform.offsetMax = new Vector2(0f, -(adHeight()));
		//	popupTransform.offsetMax = new Vector2(0f, -(adHeight()));
		//}
		//else
		//{
			uiTransform.offsetMax = Vector2.zero;
			popupTransform.offsetMax = Vector2.zero;
		//}
	}

	public void ShowNoAdsButton(bool show)
	{
		if(noAdsButton.activeSelf != show)
			noAdsButton.SetActive(show);
	}

	public void ShowOfflineMode(bool show)
	{
		if (offlineMode.activeSelf != show)
			offlineMode.SetActive(show);

		if (show)
			PopupUIManager.Instance.ShowPopupUI("OfflinePopupUI");
	}

	public float adHeight()
	{
		float aspect = (Screen.height / canvasTrans.sizeDelta.y);
		float height = AdmobManager.Instance.GetBannerHeight() / aspect;

		return height;
	}

	public void RefreshCoinInfoUI()
	{
		coinInfoUI.RefreshUI();
	}
}
