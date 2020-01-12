using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MeterInfoUI : MonoBehaviour
{
    //public Text meterText;
	public TextMeshProUGUI meterText;

    void Update()
    {
		if(GameManager.Instance.gameState == GameState.GamePlay)
	        RefreshUI();
    }

    public void RefreshUI()
    {
        Player player = GameManager.Instance.player;
        if (player == null)
            return;

        meterText.text = GetMeterText(player.height);
    }

	string GetMeterText(float height)
	{
		return string.Format("{0:f1}M", height);
	}
}
