using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeGameStateButton : BasicUIButton
{
    public GameState state;

	protected override void PressedButton()
    {
        GameManager.Instance.SetGameState(state);
    }
}
