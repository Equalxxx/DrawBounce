using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLFramework;

public class DialogueTrigger : MonoBehaviour {

    private Button myButton;

    private void Start()
    {
        myButton = GetComponent<Button>();
        myButton.onClick.AddListener(PressedBtn);
    }

    void PressedBtn()
    {
        DialogueManager.Instance.DisplayNextSentence();
    }
}
