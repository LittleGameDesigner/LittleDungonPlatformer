using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Chests : MonoBehaviour
{
    private Animator animator;
    private GameObject Player;
    public GameObject Potion;

    void Start()
    {
        animator = GetComponent<Animator>();
        animator.speed = 0;
    }

    void Update()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((Input.GetKeyDown(KeyCode.F)) && (XRange(Player) < 2) && (YRange(Player) < 2)){
            FindObjectOfType<AudioManager>().Play("OpenChest");
            animator.speed = 1;
            Potion.SetActive(true);
        }
    }

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
    }
}
