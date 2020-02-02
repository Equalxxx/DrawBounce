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
		PlayableBlock player = GameManager.Instance.curPlayableBlock;
        if (player == null)
            return;

        meterText.text = UnitCalculation.GetHeightText(player.height, true);
    }
}
