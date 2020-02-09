using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPopupUI : ProtoPopupUI
{
	public override void InitPopupUI()
	{
		popupUIType = PopupUIType.Option;
		GameManager.Instance.curPlayableBlock.Show(false);
	}

	public override void RefreshUI()
	{

	}

	public override void ClosePopupUI()
	{
		GameManager.Instance.curPlayableBlock.Show(true);
	}
}
