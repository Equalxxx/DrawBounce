using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MysticLights;

public enum WaitingPopupType { Saving, Loading }

public class UIManager : Singleton<UIManager>
{
    public UIGroup[] uiGroups;
    public UIGroup currentUIGroup;
	public RectTransform uiTransform;
	public RectTransform canvasTrans;
	public float fadeDuration = 1f;
	
	public ShowMessageUI showMessageUI;
	public GameObject practiceUI;
	public GameObject noAdsButton;

	public CoinInfoUI coinInfoUI;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			ShowPopup(PopupUIType.Quit, true);
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
				case PopupUIType.Waiting:
					PopupUIManager.Instance.ShowPopupUI("WaitingPopupUI");
					break;
				case PopupUIType.NoAds:
					PopupUIManager.Instance.ShowPopupUI("NoAdsPopupUI");
					break;
				case PopupUIType.Practice:
					PopupUIManager.Instance.ShowPopupUI("PracticePopupUI");
					break;
			}
		}
		else
		{
			if(PopupUIManager.Instance.curPopupUI.popupUIType == popupType)
				PopupUIManager.Instance.ClosePopupUI();
		}
	}

	public void ShowPracticeUI(bool show)
	{
		if (practiceUI.activeSelf != show)
			practiceUI.SetActive(show);
	}

	public void SetUIRects()
	{
		if (!GameManager.IsNoAds && GameManager.IsConnected)
		{
			uiTransform.offsetMax = new Vector2(0f, -(adHeight()));
		}
		else
		{
			uiTransform.offsetMax = Vector2.zero;
		}
	}

	public void ShowNoAdsButton(bool show)
	{
		if(noAdsButton.activeSelf != show)
			noAdsButton.SetActive(show);
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
