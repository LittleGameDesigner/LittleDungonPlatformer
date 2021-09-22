using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkeletonMage : MonoBehaviour
{
    //Move
    private float TurnOverTime = 0;
    private int direction = 1;
    public float MoveSpeed = 1;
    //Attack
    public Transform attackPoint;
    public LayerMask EnemyLayer;
    public float attackRange = 30;
    public float AttackCD = 0.5f;
    public float AttackCoolDown = 3;
    public float AttackDemage = 30;
    public float timeSinceLastAttack;
    //Material
    private Rigidbody2D rb;
    private GameObject Player;
    public GameObject SpellObject;
    private Animator animator;
    public float exp;
    //Health
    public float Health;
    public float maxHealth = 20;
    private Color originalColor;
    public HealthBarBehaviour healthBar;
    private bool dead;

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
        if((XRange(Player) < 30) && (YRange(Player) < 10)){
            ChangeDirection();
            transform.Translate(Vector3.right * 0, Space.World);
            
            if(AttackCD > AttackCoolDown){
                animator.SetBool("isRunning", false);
                animator.SetTrigger("Attack");
                AttackCD = 0;
                Invoke("Attack", 0.7f);
            }        
        }
        AttackCD += Time.fixedDeltaTime;
    }

    private void Attack(){
        FindObjectOfType<AudioManager>().Play("SwordSwing");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 100, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            enemy.SendMessage("TakeDemage", AttackDemage, SendMessageOptions.DontRequireReceiver);
            FindObjectOfType<AudioManager>().Play("Electric");
            Instantiate(SpellObject, enemy.transform.position, transform.rotation);
        }
        animator.SetTrigger("StopAttack");
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
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);
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
