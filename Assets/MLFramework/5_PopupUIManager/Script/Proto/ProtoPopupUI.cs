using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PopupUIType { Quit, Pause, Tutorial, Waiting, NoAds, Practice }

public abstract class ProtoPopupUI : MonoBehaviour {

	public PopupUIType popupUIType;
	public abstract void InitPopupUI();
    public abstract void RefreshUI();
	public abstract void ClosePopupUI();

    public virtual void ShowPopupUI(bool show)
    {
		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);
    }
}
