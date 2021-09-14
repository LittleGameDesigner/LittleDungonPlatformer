using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Slime : MonoBehaviour
{
    //Move
    private float TurnOverTime = 0;
    private int direction = 1;
    public float MoveSpeed = 2;
    public bool onPatrol = true;
    //Attack
    //public Transform attackPoint;
    public LayerMask EnemyLayer;
    //private bool isAttacking;
    //public float attackRange = 1;
    public float AttackCD = 0.5f;
    public float AttackDemage;
    //public float timeSinceLastAttack;
    //Material
    private GameObject Player;
    private Animator animator;
    public float exp;
    //Health
    public float Health;
    public float maxHealth;
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

    private void FixedUpdate() {
        if(dead){return;}
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if ((XRange(Player) > 0.5f) || (YRange(Player) > 0.5f))
        {
            Chase();
        } 
        if(TurnOverTime > 3){
            direction *= -1;
            TurnOverTime = 0;
        }
        TurnOverTime += Time.fixedDeltaTime;
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

    private void OnTriggerStay2D(Collider2D collision){
        if(collision.tag == "Player"){
            print("a");
            if(AttackCD > 0.2f){
                FindObjectOfType<AudioManager>().Play("SlimeAttack");
                collision.SendMessage("TakeDemage", AttackDemage, SendMessageOptions.DontRequireReceiver);
                AttackCD = 0;
            }
            AttackCD += Time.deltaTime;
        }
        
    }

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
    }

    private void Chase(){
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((XRange(Player) < 10) && (YRange(Player) < 4)){
            MoveSpeed = 5;
            if(Player.transform.position.x < transform.position.x){
                transform.Translate(Vector3.right * MoveSpeed * -1 * Time.fixedDeltaTime, Space.World);
            }else if (Player.transform.position.x >= transform.position.x){
                transform.Translate(Vector3.right * MoveSpeed * 1 * Time.fixedDeltaTime, Space.World);
            }
        }else{
            MoveSpeed = 2;
            if(onPatrol){
                Move();
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
