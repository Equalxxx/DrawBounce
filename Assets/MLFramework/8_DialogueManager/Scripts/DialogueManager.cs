using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MysticLights
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        public bool isDone;
        public Queue<int> sentences;

        public BaseDialogueSystem curDialogueSystem;

        private void Start()
        {
            sentences = new Queue<int>();
            curDialogueSystem.ShowDialogue(false);
        }

        public void StartDialogue(Dialogue dialogue)
        {
            isDone = false;
            sentences.Clear();

            foreach (int sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            curDialogueSystem.ShowDialogue(true);
            curDialogueSystem.StartDialogue(dialogue);
            DisplayNextSentence();
        }

        public void DisplayNextSentence()
        {
            if (sentences.Count == 0)
            {
                EndDialogue();
                return;
            }

            curDialogueSystem.DisplayNextSentence(sentences.Dequeue());
        }

        void EndDialogue()
        {
            curDialogueSystem.EndDialogue();
            curDialogueSystem.ShowDialogue(false);
            isDone = true;
        }
    }
}
