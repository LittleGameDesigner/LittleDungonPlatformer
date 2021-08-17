using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemySword : MonoBehaviour
{
    //Move
    private float TurnOverTime = 0;
    private int direction = 1;
    public float MoveSpeed = 2;
    //Attack
    public Transform attackPoint;
    public LayerMask EnemyLayer;
    public float attackRange = 1;
    public float AttackCD = 0.5f;
    public float AttackDemage = 5;
    public float timeSinceLastAttack;
    //Material
    private GameObject Player;
    private Animator animator;
    //Health
    public float Health;
    public float maxHealth = 20;
    public HealthBarBehaviour healthBar;


    void Start()
    {
        animator = GetComponent<Animator>();
        Health = maxHealth;
        //healthBar.Active(true);
        healthBar.SetMaxHealth(maxHealth);
        healthBar.Active(false);
    }

    // Update is called once per frame
    void Update()
    {
        //healthBar.SetHealth(Health, maxHealth);
    }

    private void FixedUpdate() {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(Math.Abs(Player.transform.position.x - transform.position.x) < 2){
            transform.Translate(Vector3.right * 0, Space.World);
            
            if(AttackCD > 2){
                animator.SetBool("isRunning", false);
                animator.SetTrigger("Attack");
                AttackCD = 0;
                Invoke("Attack", 0.3f);
            }        
        }else{
            Detected();
        }
        if(TurnOverTime > 3){
            direction *= -1;
            TurnOverTime = 0;
            //animator.SetTrigger("Attack1");
        }
        TurnOverTime += Time.fixedDeltaTime;
        AttackCD += Time.fixedDeltaTime;
    }

    private void Move(){
        
        if(TurnOverTime > 3){
            Vector3 facingDirection = transform.localScale;
            facingDirection.x *= -1;
            transform.localScale = facingDirection;
        }
        transform.Translate(Vector3.right * MoveSpeed * direction * Time.fixedDeltaTime, Space.World);

    }

    private void Attack(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            enemy.SendMessage("TakeDemage", AttackDemage, SendMessageOptions.DontRequireReceiver);
            FindObjectOfType<AudioManager>().Play("AttackMelee");
        }
        timeSinceLastAttack = Time.time;
    }

    private void Detected(){
        if(timeSinceLastAttack + 1 > Time.time){return;}
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        animator.SetBool("isRunning", true);
        if(Math.Abs(Player.transform.position.x - transform.position.x) < 8){
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
            MoveSpeed = 2;
            Move();
        }
    }

    private void TakeDemage(float demage){
        Health -= demage;
        healthBar.Active(true);
        healthBar.SetHealth(Health);
        if(Health <= 0){
            animator.SetTrigger("Die");
            Destroy(gameObject, 0.5f);
        }
    }
}

