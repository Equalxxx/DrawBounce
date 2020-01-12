using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class OptionButton : MonoBehaviour
{
	private Button button;
	private bool isShow;
	private DOTweenAnimation buttonAnim;

	public MuteButton muteButton;

	private void Awake()
	{
		button = GetComponent<Button>();
		buttonAnim = GetComponent<DOTweenAnimation>();
	}

	private void OnEnable()
	{
		button.onClick.AddListener(PressedButton);
	}

	private void OnDisable()
	{
		button.onClick.RemoveListener(PressedButton);
	}

	void PressedButton()
	{
		isShow = !isShow;

		if(isShow)
		{
			buttonAnim.DOPlayForward();
			muteButton.ShowButton(true);
		}
		else
		{
			buttonAnim.DOPlayBackwards();
			muteButton.ShowButton(false);
		}
	}
}
