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
		PlayableBlock player = GameManager.Instance.player;
        if (player == null)
            return;

        meterText.text = GetHeightText(player.height);
    }

	string GetHeightText(float height)
	{
		string distText = "";

		if(height >= 1000f)
		{
			float kilo = height / 1000f;
			distText = string.Format("{0:f2}KM", kilo);
		}
		else
		{
			distText = string.Format("{0:f2}M", height);
		}

		return distText;
	}
}
