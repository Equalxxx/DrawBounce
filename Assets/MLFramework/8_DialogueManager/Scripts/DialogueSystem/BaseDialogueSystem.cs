using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDialogueSystem : MonoBehaviour {
    public abstract void StartDialogue(Dialogue dialogue);
    public abstract void DisplayNextSentence(int stringIndex);
    public abstract void EndDialogue();
    public abstract void ShowDialogue(bool show);
}
