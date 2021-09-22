using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkeletonShield : MonoBehaviour
{
    //Move
    private float TurnOverTime = 0;
    private int direction = 1;
    //Attack
    public Transform attackPoint;
    public LayerMask EnemyLayer;
    private bool isAttacking;
    private bool useCrescentMoon;
    public float attackRange;
    public float AttackCD;
    public float AttackDemage;
    public float timeSinceLastAttack;
    private int skills;
    System.Random rnd = new System.Random();
    private bool isSwordAttacking;
    private bool isPushing;
    private Vector3 eulerAngles;
    //Material
    private Rigidbody2D rb;
    public GameObject CrescentMoon;
    private GameObject Player;
    private Animator animator;
    public float exp;
    //Health
    public float Health;
    public float maxHealth = 20;
    public HealthBarBehaviour healthBar;
    private float block;
    private bool dead;
    private Color originalColor;


    void Start()
    {
        animator = GetComponent<Animator>();
        Health = maxHealth;
        healthBar.SetMaxHealth(Health, maxHealth);
        healthBar.Active(false);
        rb = GetComponent<Rigidbody2D>();
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        originalColor = PlayerRenderrer.material.color;
    }

    private void FixedUpdate() {
        if(dead){return;}
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(!isAttacking)ChangeDirection();
        if((XRange(Player) < 2) && (YRange(Player) < 2)){
            transform.Translate(Vector3.right * 0, Space.World);
            
            if(AttackCD > 1){
                skills = rnd.Next(1,6);
                isAttacking = true;
                if (skills <= 2)
                {
                    animator.SetTrigger("Push");
                    isPushing = true;
                }
                else if(skills >= 4){
                    animator.SetTrigger("Attack");
                    isSwordAttacking = true;
                }else{
                    animator.SetTrigger("Attack");
                    useCrescentMoon = true;
                }
                AttackCD = 0;
                Invoke("Attack", 0.8f);
            }        
        }
        if(TurnOverTime > 3){
            direction *= -1;
            TurnOverTime = 0;
        }
        TurnOverTime += Time.fixedDeltaTime;
        AttackCD += Time.fixedDeltaTime;
    }

    private void ChangeDirection(){
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        Vector3 facingDirection = transform.localScale;
        if(Player.transform.position.x < transform.position.x){
            if(facingDirection.x > 0){
                facingDirection.x *= -1;
            }
            eulerAngles = new Vector3(0, 0, -180);
            transform.localScale = facingDirection;
        }else{
            if(facingDirection.x < 0){
                facingDirection.x *= -1;
            }
            eulerAngles = new Vector3(0, 0, 0);
            transform.localScale = facingDirection;
        }
    }

    private void Attack(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);
        if (isSwordAttacking)
        {
            foreach (Collider2D enemy in hitEnemies){
                enemy.SendMessage("TakeDemage", AttackDemage, SendMessageOptions.DontRequireReceiver);
                FindObjectOfType<AudioManager>().Play("AttackMelee");
            }
            timeSinceLastAttack = Time.time;
            animator.SetTrigger("StopAttack");
            isSwordAttacking = false;
        }

        else if(isPushing){
            foreach (Collider2D enemy in hitEnemies){
                enemy.SendMessage("PushBack", direction * 15, SendMessageOptions.DontRequireReceiver);
                enemy.SendMessage("TakeDemage", AttackDemage, SendMessageOptions.DontRequireReceiver);
                FindObjectOfType<AudioManager>().Play("AttackMelee");
            }
            timeSinceLastAttack = Time.time;
            animator.SetTrigger("StopPush");
            isPushing = false;
        }

        else if(useCrescentMoon){
            Instantiate(CrescentMoon, transform.position, Quaternion.Euler(transform.eulerAngles + eulerAngles));
            animator.SetTrigger("StopAttack");
            useCrescentMoon = false;
        }
        
        isAttacking = false;
    }

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
    }

    private void TakeDemage(float demage){
        if(dead)return;
        block = rnd.Next(1,4);
        if(block == 1){
            FindObjectOfType<AudioManager>().Play("Block");
            return;
        }
        Health -= demage;
        healthBar.Active(true);
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", Color.red);
        Invoke("DemageEffect", 0.2f);
        healthBar.SetHealth(Health);
        if(Health <= 0){
            dead = true;
            animator.SetTrigger("Die");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 100, EnemyLayer);
            foreach (Collider2D enemy in hitEnemies){
                enemy.SendMessage("GainEXP", exp, SendMessageOptions.DontRequireReceiver);
            }
            FindObjectOfType<AudioManager>().Play("SkeletonDie");
        }else{
            animator.speed = 0;
            FindObjectOfType<AudioManager>().Play("Hurt");
            Invoke("PlayAnimation", 0.2f);        
        }
    }

    private void PushBack(int d){
        animator.SetTrigger("PushBack");
        rb.velocity = new Vector2(d, rb.velocity.y);
    }

    private void PushUp(int d){
        rb.AddForce(new Vector2(0, d), ForceMode2D.Impulse);
    }

    private void DemageEffect(){
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", originalColor);
    }


    private void PlayAnimation(){
        animator.speed = 1;
    }

}

