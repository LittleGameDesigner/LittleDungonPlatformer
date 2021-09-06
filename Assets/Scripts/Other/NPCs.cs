using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class NPCs : MonoBehaviour
{
    private GameObject Player;
    private DialogueTrigger dialogueTrigger;
    System.Random rnd = new System.Random();
    private bool dialogueStarted;
    private bool dialogueFinished;
    private int talk;
    private int previousTalk;

    void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    void Update()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((XRange(Player) < 4) && (YRange(Player) < 3) && !dialogueStarted){
            if(dialogueFinished){
                dialogueTrigger.TriggerLastSentence();
            }else{
                dialogueTrigger.TriggerDialogue();
            }
            FindObjectOfType<AudioManager>().Play("BabbleTalk" + BabbleTalk().ToString());
            dialogueStarted = true;
        }
        if(Input.GetKeyDown(KeyCode.Return) && dialogueStarted){
            if(dialogueTrigger.TriggerNextSentence() == 0){
                FindObjectOfType<AudioManager>().Play("BabbleTalk" + BabbleTalk().ToString());
            }else{
                dialogueFinished = true;
            }
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

    private int BabbleTalk(){
        talk = rnd.Next(1, 7);
        if(talk == previousTalk){
            talk += 1;
            if(talk >= 7){talk = 1;}
        }
        previousTalk = talk;
        return talk;
    }
}
