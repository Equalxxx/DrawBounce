using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseButton : MonoBehaviour
{
	private Button myButton;
	public bool pauseSwitch;

	private void Awake()
	{
		myButton = GetComponent<Button>();
	}

	private void OnEnable()
	{
		myButton.onClick.AddListener(PressedButton);
	}

	private void OnDisable()
	{
		myButton.onClick.RemoveListener(PressedButton);
	}

	void PressedButton()
	{
		GameManager.Instance.SetPause(pauseSwitch);
	}
}
