using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeGameStateBtn : MonoBehaviour
{
    private Button button;
    public GameState state;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(PressedButton);
    }

    void PressedButton()
    {
        GameManager.Instance.SetGameState(state);
    }
}
