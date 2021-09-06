using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    public void TriggerDialogue(){
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    public int TriggerNextSentence(){
        return FindObjectOfType<DialogueManager>().DisplayNextSentence();
    }

    public void TriggerLastSentence(){
        FindObjectOfType<DialogueManager>().DisplayLastSentence();
    }

    public void TriggerDialogueEnd(){
        FindObjectOfType<DialogueManager>().EndDialogue();
    }
}
