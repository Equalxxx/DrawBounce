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

	private void Awake()
	{
		if(myButton == null)
			myButton = GetComponent<Button>();

		InitButton();
	}

	protected virtual void OnEnable()
	{
		myButton.onClick.AddListener(PressedButton);
	}

	protected virtual void OnDisable()
	{
		myButton.onClick.RemoveListener(PressedButton);
	}

	protected abstract void InitButton();
	protected abstract void PressedButton();
}
