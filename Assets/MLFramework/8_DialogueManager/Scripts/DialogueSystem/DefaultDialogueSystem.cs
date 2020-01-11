using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MysticLights;

public class DefaultDialogueSystem : BaseDialogueSystem
{
    public Text nameText;
    public Text dialogueText;
    public GameObject dialogueTrigger;
    public float typingDelay = 0.1f;

    public override void StartDialogue(Dialogue dialogue)
    {
        //
        // Dialog window open anim
        //

        dialogueText.text = "";
        nameText.text = dialogue.name;

        ShowTrigger(false);
    }

    public override void DisplayNextSentence(int stringIndex)
    {
        ShowTrigger(false);

        string sentence = LocalizeManager.GetStr(stringIndex);

        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    public override void EndDialogue()
    {
        ShowTrigger(false);

        dialogueText.text = "";
        nameText.text = "";
    }

    public override void ShowDialogue(bool show)
    {
        if (gameObject.activeSelf != show)
            gameObject.SetActive(show);
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingDelay);
        }

        ShowTrigger(true);
    }

    void ShowTrigger(bool show)
    {
        if (dialogueTrigger.activeSelf != show)
            dialogueTrigger.SetActive(show);
    }

}
