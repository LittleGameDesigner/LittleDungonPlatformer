using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spider : MonoBehaviour
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
    public float exp;
    //Health
    public float Health;
    public float maxHealth = 12;
    public HealthBarBehaviour healthBar;
    private bool dead;
    private Color originalColor;


    void Start()
    {
        animator = GetComponent<Animator>();
        Health = maxHealth;
        healthBar.SetMaxHealth(Health, maxHealth);
        healthBar.Active(false);
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        originalColor = PlayerRenderrer.material.color;
    }

    void Update()
    {
    }

    private void FixedUpdate() {
        if(dead == true){return;}
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((XRange(Player) < 1.6f) && (YRange(Player) < 1.6f)){
            transform.Translate(Vector3.right * 0, Space.World);
            animator.SetBool("isRunning", false);

            if(AttackCD > 0.9f){
                animator.SetTrigger("Attack");
                AttackCD = 0;
                Invoke("Attack", 0.4f);
            }        
        }else{
            Chase();
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

    private void Attack(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            enemy.SendMessage("TakeDemage", AttackDemage, SendMessageOptions.DontRequireReceiver);
            FindObjectOfType<AudioManager>().Play("AttackMelee");
        }
        timeSinceLastAttack = Time.time;
        animator.SetTrigger("Attack");
    }

    private void Chase(){
        if(timeSinceLastAttack + 1 > Time.time){return;}
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((XRange(Player) < 10) && (YRange(Player) < 5)){
            animator.SetBool("isRunning", true);
            MoveSpeed = 12;
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
            //MoveSpeed = 2;
            //Move();
        }
    }

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
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
            FindObjectOfType<AudioManager>().Play("SpiderDie");
        }else{
            animator.speed = 0;
            FindObjectOfType<AudioManager>().Play("Hurt");
            Invoke("PlayAnimation", 0.2f);
        }
    }

    private void DemageEffect(){
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", originalColor);
    }

    private void PlayAnimation(){
        animator.speed = 1;
    }
}

