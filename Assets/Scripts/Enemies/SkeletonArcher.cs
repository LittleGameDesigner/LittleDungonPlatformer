using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkeletonArcher : MonoBehaviour
{
    //Move
    private float TurnOverTime = 0;
    private int direction = 1;
    public float MoveSpeed = 2;
    public bool isDetected;
    //Material
    private Animator animator;
    public LayerMask EnemyLayer;
    private GameObject Player;
    public float exp;
    //Attack
    public bool isAttacking;
    public GameObject EnemyArrow;
    private Vector3 EnemyArrowEulerAngles;
    private float attackCD = 1.5f;
    //Health
    public float Health;
    public float maxHealth = 20;
    private bool dead;
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
        if(dead){return;}
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((XRange(Player) < 20) && (YRange(Player) < 5)){
            if(!isAttacking)ChangeDirection();
            transform.Translate(Vector3.right * 0, Space.World);
            
            if(attackCD >= 3){
                isAttacking = true;
                animator.SetBool("isRunning", false);
                animator.SetTrigger("Attack");
                attackCD = 0;
                Attack();
            }        
        }
        attackCD += Time.fixedDeltaTime;
    }

    private void ChangeDirection(){
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        Vector3 facingDirection = transform.localScale;
        if(Player.transform.position.x < transform.position.x){
            if(facingDirection.x > 0){
                facingDirection.x *= -1;
            }
            transform.localScale = facingDirection;
            EnemyArrowEulerAngles = new Vector3 (0, 0, -180);
        }else{
            if(facingDirection.x < 0){
                facingDirection.x *= -1;
            }
            transform.localScale = facingDirection;
            EnemyArrowEulerAngles = new Vector3 (0, 0, 0);
        }
    }

    private void Attack(){
        Invoke("CreateArrow", 1.3f);
        attackCD = 0;
    }

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
    }

    private void CreateArrow(){
        Instantiate(EnemyArrow, transform.position, Quaternion.Euler(transform.eulerAngles + EnemyArrowEulerAngles));
        FindObjectOfType<AudioManager>().Play("ArrowFly");
        animator.SetTrigger("StopAttack");
        isAttacking = false;
    }

    private void TakeDemage(float demage){
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

    private void DemageEffect(){
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", originalColor);
    }

    private void PlayAnimation(){
        animator.speed = 1;
    }
}

