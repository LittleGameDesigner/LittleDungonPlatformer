using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkeletonBoss : MonoBehaviour
{
    //Move
    private float TurnOverTime = 0;
    private int direction = 1;
    public float MoveSpeed = 2;
    //Attack
    public Transform attackPoint;
    public LayerMask EnemyLayer;
    public float attackRange = 3;
    public float AttackCD = 3.5f;
    private float AttackCoolDown = 3.5f;
    public float AttackDemage = 30;
    public float timeSinceLastAttack;
    private int criticalHit;
    private bool isCharging = false;
    private bool inBerserkerState = false;
    System.Random rnd = new System.Random();
    //Material
    private GameObject Player;
    public GameObject HitEffect;
    private Animator animator;
    private bool PlayTheme = true;
    public float exp;
    //Health
    public float Health;
    public float maxHealth = 350;
    private float stunCount = 0;
    private bool Stuned;
    private Color originalColor;
    public HealthBarBehaviour healthBar;


    void Start()
    {
        animator = GetComponent<Animator>();
        Health = maxHealth;
        healthBar.SetMaxHealth(Health, maxHealth);
        healthBar.Active(false);
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        originalColor = PlayerRenderrer.material.color;
    }

    private void FixedUpdate() {
        if(stunCount > 0){
            stunCount -= Time.fixedDeltaTime;
            return;
        }
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((XRange(Player) < 16) && (YRange(Player) < 8) && PlayTheme){
            FindObjectOfType<AudioManager>().Stop("Theme");
            FindObjectOfType<AudioManager>().Play("SkeletonBossTheme");
            PlayTheme = false;
        }else if((XRange(Player) >= 30) && (YRange(Player) >= 15) && !PlayTheme){
            FindObjectOfType<AudioManager>().Stop("SkeletonBossTheme");
            FindObjectOfType<AudioManager>().Play("Theme");
            PlayTheme = true;
        }
        if((XRange(Player) < 4.5) && (YRange(Player) < 3)){
            transform.Translate(Vector3.right * 0, Space.World);
            
            if(AttackCD > AttackCoolDown){
                animator.SetBool("isRunning", false);
                AttackCD = 0;
                criticalHit = rnd.Next(1, 15);
                timeSinceLastAttack = Time.time;
                if(inBerserkerState){
                    AttackDemage = 30;
                    animator.SetTrigger("Attack2");
                    Invoke("Attack", 0.3f);
                }else if(criticalHit == 1 || criticalHit == 2){
                    print("charge");
                    animator.SetTrigger("Charge");
                    isCharging = true;
                    AttackDemage = 12;
                    Invoke("Attack", 0.1f);
                    Invoke("Attack", 0.25f);
                    Invoke("Attack", 0.45f);
                    Invoke("Attack", 0.65f);
                    Invoke("Charge", 0.8f);
                }else{
                    AttackDemage = 30;
                    animator.SetTrigger("Attack2");
                    Invoke("Attack", 0.6f);
                }
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
        FindObjectOfType<AudioManager>().Play("SwordSwing");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            enemy.SendMessage("TakeDemage", AttackDemage, SendMessageOptions.DontRequireReceiver);
            FindObjectOfType<AudioManager>().Play("SkeletonBoss");
            Instantiate(HitEffect, enemy.transform.position, transform.rotation);
        }
    }

    private void Charge(){
        FindObjectOfType<AudioManager>().Play("SwordSwing");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            enemy.SendMessage("TakeDemage", AttackDemage, SendMessageOptions.DontRequireReceiver);
            FindObjectOfType<AudioManager>().Play("SkeletonBoss");
        }
        isCharging = false;
    }

    private void Chase(){
        if((timeSinceLastAttack + 2 > Time.time) && isCharging == false){return;}
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        animator.SetBool("isRunning", true);
        if((XRange(Player) < 16) && (YRange(Player) < 8)){
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

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
    }

    private void TakeDemage(float demage){
        Health -= demage;
        healthBar.Active(true);
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", Color.red);
        Invoke("DemageEffect", 0.2f);
        healthBar.SetHealth(Health);
        if(Health < maxHealth/2 && !Stuned){
            stunCount = 3;
            Stuned = true;
            inBerserkerState = true;
            AttackCoolDown = 1;
            animator.speed = 3;
        }
        animator.speed = 0;
        FindObjectOfType<AudioManager>().Play("Hurt");
        if(Health <= 0){
            animator.speed = 1;
            animator.SetTrigger("Die");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);
            foreach (Collider2D enemy in hitEnemies){
                enemy.SendMessage("GainEXP", exp, SendMessageOptions.DontRequireReceiver);
            }
            FindObjectOfType<AudioManager>().Stop("SkeletonBossTheme");
            FindObjectOfType<AudioManager>().Play("Win");
        }
        Invoke("PlayAnimation", 0.2f);
    }

    private void DemageEffect(){
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", originalColor);
    }

    private void PlayAnimation(){
        animator.speed = 1;
        if(Health < maxHealth/2 && Health > 0){
            animator.speed = 3;
        }
    }
}

