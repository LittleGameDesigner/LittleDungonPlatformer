using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour
{
    //Materials
    public GameObject Player;
    //Door
    private bool openDoor;
    private bool PlayMusic = true;
    public bool hasKey;
    public float MoveSpeed = 1;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((XRange(Player) < 3) && (YRange(Player) < 3) && PlayMusic && hasKey){
            openDoor = true;
            FindObjectOfType<AudioManager>().Play("OpenDoor");
            PlayMusic = false;
        }
        if(openDoor){
            transform.Translate(Vector3.up * MoveSpeed * Time.fixedDeltaTime,Space.World);
            Invoke("Destroy", 3);
        }
    }

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
    }

    private void Destroy(){
        Destroy(gameObject);
    }
}
