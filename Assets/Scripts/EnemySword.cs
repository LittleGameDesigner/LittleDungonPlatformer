using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySword : MonoBehaviour
{
    private float TurnOverTime = 0;
    private int direction = 1;
    public float MoveSpeed = 2;
    public bool isDetected;
    private Animator animator;
    //Vector3 facingDirection = transform.localScale;

    private GameObject Player;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        Detected();
        if(!isDetected){
            Move();
        }
        if(TurnOverTime > 3){
            direction *= -1;
            TurnOverTime = 0;
            animator.SetTrigger("Attack1");
        }
        TurnOverTime += Time.fixedDeltaTime;
    }

    private void Move(){
        
        if(TurnOverTime > 3){
            Vector3 facingDirection = transform.localScale;
            facingDirection.x *= -1;
            transform.localScale = facingDirection;
        }
        transform.Translate(Vector3.right * MoveSpeed * direction * Time.fixedDeltaTime, Space.World);

    }

    private void Detected(){
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(Math.Abs(Player.transform.position.x - transform.position.x) < 8){
            isDetected = true;
            MoveSpeed = 4;
            Vector3 facingDirection = transform.localScale;
            if(Player.transform.position.x < transform.position.x){
                if(facingDirection.x > 0){
                    facingDirection.x *= -1;
                }
                transform.localScale = facingDirection;
                transform.Translate(Vector3.right * MoveSpeed * -1 * Time.fixedDeltaTime, Space.World);
            }else{
                if(facingDirection.x < 0){
                    facingDirection.x *= -1;
                }
                transform.localScale = facingDirection;
                transform.Translate(Vector3.right * MoveSpeed * 1 * Time.fixedDeltaTime, Space.World);
            }
        }else{
            isDetected = false;
            MoveSpeed = 2;
        }
    }
}

