using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCs : MonoBehaviour
{
    private GameObject Player;
    private DialogueTrigger dialogueTrigger;
    private bool dialogueStarted;

    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    void Update()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((XRange(Player) < 4) && (YRange(Player) < 3) && !dialogueStarted){
            dialogueTrigger.TriggerDialogue();
            dialogueStarted = true;
        }
        if(Input.GetKeyDown(KeyCode.Return) && dialogueStarted){
            dialogueTrigger.TriggerNextSentence();
        }

        if(((XRange(Player) > 8) || (YRange(Player) > 6)) && dialogueStarted){
            dialogueTrigger.TriggerDialogueEnd();
            dialogueStarted = false;
        }
    }

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
    }
}
