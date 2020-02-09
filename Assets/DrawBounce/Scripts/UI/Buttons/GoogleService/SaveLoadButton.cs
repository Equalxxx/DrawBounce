using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLFramework;

public class SaveLoadButton : BasicUIButton
{
	public enum DataHandleType { Save, Load }
	public DataHandleType handleType;

	protected override void PressedButton()
	{
		if(GameManager.IsConnected)
		{
			if(GameManager.IsOfflineMode)
			{
				UIManager.Instance.showMessageUI.Show(12);
				SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
				return;
			}

			switch(handleType)
			{
				case DataHandleType.Save:
					GameManager.Instance.gameSettings.SaveGameData(true);
					UIManager.Instance.ShowPopup(PopupUIType.SaveData, false);
					break;
				case DataHandleType.Load:
					GameManager.Instance.gameSettings.LoadGameData(true);
					UIManager.Instance.ShowPopup(PopupUIType.LoadData, false);
					break;
			}
		}
		else
		{
			UIManager.Instance.showMessageUI.Show(10);
			SoundManager.Instance.PlaySound2D("Buy_Item_Notwork");
		}
	}
}
