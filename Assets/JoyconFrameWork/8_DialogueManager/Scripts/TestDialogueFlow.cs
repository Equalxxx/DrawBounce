using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoyconFramework;

public class TestDialogueFlow : MonoBehaviour {

    public TestDialogue[] testDialogues;

    private void Start()
    {
        testDialogues = GetComponentsInChildren<TestDialogue>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            StartCoroutine(ShowDialogues());
    }

    IEnumerator ShowDialogues()
    {
        yield return StartDialogue(0);

        yield return new WaitForSeconds(0.5f);

        yield return StartDialogue(1);

        yield return new WaitForSeconds(0.5f);

        yield return StartDialogue(2);

        yield return new WaitForSeconds(0.5f);

        yield return StartDialogue(3);
    }

    IEnumerator StartDialogue(int idx)
    {
        DialogueManager.Instance.StartDialogue(testDialogues[idx].dialogue);
        while (!DialogueManager.Instance.isDone)
        {
            yield return null;
        }
    }
}
