using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLFramework;

public class DeviceOptionButton : BasicUIButton
{
	public enum DeviceOptionType { BGM, SE, Vibe }
	public DeviceOptionType optionType;

	public bool toggle;

	public RectTransform buttonTrans;
	public Image stateImage;
	public Text stateText;
	public float targetPosition1;
	public float targetPosition2;

	protected override void InitButton()
	{
		DeviceSettings deviceSettings = GameManager.Instance.deviceSettings;

		switch (optionType)
		{
			case DeviceOptionType.BGM:
				toggle = deviceSettings.useBGM;
				break;
			case DeviceOptionType.SE:
				toggle = deviceSettings.useSE;
				break;
			case DeviceOptionType.Vibe:
				toggle = deviceSettings.viberate;
				break;
		}

		RefreshUI();
	}

	protected override void PressedButton()
	{
		toggle = !toggle;

		DeviceSettings deviceSettings = GameManager.Instance.deviceSettings;

		switch (optionType)
		{
			case DeviceOptionType.BGM:
				GameManager.Instance.SetSoundSetting(SoundType.BGM, toggle);
				break;
			case DeviceOptionType.SE:
				GameManager.Instance.SetSoundSetting(SoundType.SE, toggle);
				break;
			case DeviceOptionType.Vibe:
				deviceSettings.viberate = toggle;
				break;
		}

		RefreshUI();

		SoundManager.Instance.PlaySound2D("Click");
		GameManager.Instance.gameSettings.SaveDeviceOptions();
	}

	void RefreshUI()
	{
		if(toggle)
		{
			buttonTrans.localPosition = new Vector2(-targetPosition1, 0f);
			stateImage.color = Color.green;
			stateText.rectTransform.localPosition = new Vector2(targetPosition2, 0f);
			stateText.text = "ON";
		}
		else
		{
			buttonTrans.localPosition = new Vector2(targetPosition1, 0f);
			stateImage.color = Color.gray;
			stateText.rectTransform.localPosition = new Vector2(-targetPosition2, 0f);
			stateText.text = "OFF";
		}
	}
}
