using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BossDoor : MonoBehaviour
{
    //Materials 
    private GameObject Player;
    private float doorYCoordinate;
    //Door
    public bool openDoor;
    public bool closeDoor;
    private bool PlayMusic = true;
    private float openDoorTime;
    public bool hasKey;
    public float MoveSpeed = 1;

    void Start()
    {
        doorYCoordinate = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((XRange(Player) < 3) && (YRange(Player) < 3) && PlayMusic && hasKey && !closeDoor){
            openDoor = true;
            FindObjectOfType<AudioManager>().Play("OpenDoor");
            PlayMusic = false;
        }
        if(openDoor){
            if(openDoorTime < 3){
            transform.Translate(Vector3.up * MoveSpeed * Time.fixedDeltaTime,Space.World);
            }else{
                openDoor = false;
                openDoorTime = 0;
            }
            openDoorTime += Time.deltaTime;
        }
        if(closeDoor){
            CloseDoor();
        }
    }

    private void CloseDoor(){
        if(transform.position.y > doorYCoordinate){
            transform.Translate(Vector3.up * MoveSpeed * -2 * Time.fixedDeltaTime,Space.World);
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
