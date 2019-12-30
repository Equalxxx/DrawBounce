using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeterInfoUI : BaseGameUI
{
    public Text meterText;

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
