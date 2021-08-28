using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue(){
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public void TriggerNextSentence(){
        FindObjectOfType<DialogueManager>().DisplayNextSentence();
    }

    public void TriggerDialogueEnd(){
        FindObjectOfType<DialogueManager>().EndDialogue();
    }
}
