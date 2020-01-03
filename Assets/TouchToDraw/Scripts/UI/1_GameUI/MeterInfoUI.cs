using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeterInfoUI : BaseGameUI
{
    //public Text meterText;
	public TextMeshProUGUI meterText;

    void Update()
    {
        RefreshUI();
    }

    public override void RefreshUI()
    {
        Player player = GameManager.Instance.player;
        if (player == null)
            return;

        meterText.text = string.Format("{0:f1}m", player.height);
    }
}
