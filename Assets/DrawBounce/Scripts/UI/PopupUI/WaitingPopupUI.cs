using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaitingPopupUI : MonoBehaviour
{
	public TextMeshProUGUI waitingText;
	public string savingText = "Saving...";
	public string loadingText = "Loading...";

	public void ShowPopupUI(bool show)
	{
		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);

		if(show)
			RefreshUI();
	}

	void RefreshUI()
	{
		//switch(waitingType)
		//{
		//	case WaitingPopupType.Saving:
		//		waitingText.text = savingText;
		//		break;
		//	case WaitingPopupType.Loading:
		//		waitingText.text = loadingText;
		//		break;
		//}

		waitingText.text = loadingText;
	}
}
