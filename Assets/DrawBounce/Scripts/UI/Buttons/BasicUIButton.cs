using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class BasicUIButton : MonoBehaviour
{
	public Button myButton;

	private void OnValidate()
	{
		if (myButton == null)
			myButton = GetComponent<Button>();
	}

	protected virtual void OnEnable()
	{
		if (myButton == null)
			myButton = GetComponent<Button>();

		myButton.onClick.AddListener(PressedButton);

		InitButton();
	}

	protected virtual void OnDisable()
	{
		if (myButton == null)
			myButton = GetComponent<Button>();

		myButton.onClick.RemoveListener(PressedButton);
	}

	protected abstract void InitButton();
	protected abstract void PressedButton();
}
