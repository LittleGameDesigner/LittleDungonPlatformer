using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkeletonSpear : MonoBehaviour
{
    //Move
    private float TurnOverTime = 0;
    private int direction = 1;
    public float MoveSpeed = 2;
    public bool onPatrol = true;
    //Attack
    public Transform attackPoint;
    public LayerMask EnemyLayer;
    private bool isAttacking;
    public float attackRange = 3;
    public float AttackCD = 0.5f;
    public float AttackDemage = 15;
    public float timeSinceLastAttack;
    //Material
    private Rigidbody2D rb;
    private GameObject Player;
    private Animator animator;
    public float exp;
    //Health
    public float Health;
    public float maxHealth = 20;
    public HealthBarBehaviour healthBar;
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

    void Update()
    {
    }

    private void FixedUpdate() {
        if(dead){return;}
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(!isAttacking)ChangeDirection();
        if((XRange(Player) < 3.5) && (YRange(Player) < 2)){
            transform.Translate(Vector3.right * 0, Space.World);
            
            if(AttackCD > 3){
                isAttacking = true;
                animator.SetBool("isRunning", false);
                animator.SetTrigger("Attack");
                AttackCD = 0;
                Invoke("Attack", 0.6f);
            }        
        }else{
            if(!isAttacking)Chase();
        }
        if(TurnOverTime > 3){
            direction *= -1;
            TurnOverTime = 0;
        }
        TurnOverTime += Time.fixedDeltaTime;
        AttackCD += Time.fixedDeltaTime;
    }

    private void Move(){
        
        Vector3 facingDirection = transform.localScale;
        if(direction > 0){
            if(facingDirection.x < 0){
                facingDirection.x *= -1;
            }
        }else if(direction < 0){
            if(facingDirection.x > 0){
                facingDirection.x *= -1;
            }
        }
        transform.localScale = facingDirection;
        transform.Translate(Vector3.right * MoveSpeed * direction * Time.fixedDeltaTime, Space.World);

    }

    private void ChangeDirection(){
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        Vector3 facingDirection = transform.localScale;
        if(Player.transform.position.x < transform.position.x){
            if(facingDirection.x > 0){
                facingDirection.x *= -1;
            }
            transform.localScale = facingDirection;
        }else{
            if(facingDirection.x < 0){
                facingDirection.x *= -1;
            }
            transform.localScale = facingDirection;
        }
    }

    private void Attack(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            enemy.SendMessage("TakeDemage", AttackDemage, SendMessageOptions.DontRequireReceiver);
            FindObjectOfType<AudioManager>().Play("AttackMelee");
        }
        timeSinceLastAttack = Time.time;
        animator.SetTrigger("StopAttack");
        isAttacking = false;
    }

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
    }

    private void Chase(){
        if(timeSinceLastAttack + 1 > Time.time){return;}
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((XRange(Player) < 12) && (YRange(Player) < 4)){
            animator.SetBool("isRunning", true);
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
            if(onPatrol){
                animator.SetBool("isRunning", true);
                Move();
            }else{
                animator.SetBool("isRunning", false);
            }
        }
    }

    private void TakeDemage(float demage){
        if(dead)return;
        Health -= demage;
        healthBar.Active(true);
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", Color.red);
        Invoke("DemageEffect", 0.2f);
        healthBar.SetHealth(Health);
        if(Health <= 0){
            dead = true;
            animator.SetTrigger("Die");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 100, EnemyLayer);
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

