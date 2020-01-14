using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpInfoUI : MonoBehaviour
{
    private Image hpImage;
	private TextMeshProUGUI hpText;

	private void Awake()
	{
		hpImage = GetComponentInChildren<Image>();
		hpText = GetComponentInChildren<TextMeshProUGUI>();
	}

	private void OnEnable()
    {
		GameManager.GamePlayAction += RefreshUI;
		PlayableBlock.DamagedAction += RefreshUI;
    }

    private void OnDisable()
    {
        GameManager.GamePlayAction -= RefreshUI;
		PlayableBlock.DamagedAction -= RefreshUI;
    }

    public void RefreshUI()
    {
        int hp = GameManager.Instance.player.HP;

		hpText.text = string.Format("x {0}",hp);
	}
}
