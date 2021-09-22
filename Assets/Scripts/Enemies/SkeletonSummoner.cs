using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkeletonSummoner : MonoBehaviour
{
    //Move
    private float TurnOverTime = 0;
    private int direction = 1;
    //Attack
    public LayerMask EnemyLayer;
    public float attackRange;
    private float AttackCD = 4;
    public float AttackCoolDown;
    public float timeSinceLastAttack;
    private int summonSelection;
    System.Random rnd = new System.Random();
    public GameObject SkeletonSword;
    public GameObject SkeletonArcher;
    public GameObject Spider;
    public GameObject SkeletonSpear;
    public GameObject Slime;
    //Material
    private Rigidbody2D rb;
    private GameObject Player;
    public GameObject SummonEffect;
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
        if((XRange(Player) < 20) && (YRange(Player) < 10)){
            ChangeDirection();
            transform.Translate(Vector3.right * 0, Space.World);
            
            if(AttackCD > AttackCoolDown){
                animator.SetTrigger("Summon");
                AttackCD = 0;
                Invoke("Summon", 0.7f);
            }        
        }
        AttackCD += Time.fixedDeltaTime;
    }

    private void Summon(){
        print("Summon");
        summonSelection = rnd.Next(1, 6);
        var summonEffect = Instantiate(SummonEffect, transform.position + new Vector3(0, -0.5f, 0), transform.rotation);
        FindObjectOfType<AudioManager>().Play("Summon");
        if (summonSelection == 1)
        {
            Instantiate(SkeletonSword, transform.position, Quaternion.Euler(transform.eulerAngles));
        }
        else if (summonSelection == 2)
        {
            Instantiate(SkeletonArcher, transform.position, Quaternion.Euler(transform.eulerAngles));
        }
        else if (summonSelection == 3)
        {
            Instantiate(SkeletonSpear, transform.position, Quaternion.Euler(transform.eulerAngles));
        }
        else if (summonSelection == 4)
        {
            Instantiate(Spider, transform.position, Quaternion.Euler(transform.eulerAngles));
        }
        else if (summonSelection == 5)
        {
            Instantiate(Slime, transform.position + new Vector3(0, -0.5f, 0), Quaternion.Euler(transform.eulerAngles));
        }
        animator.SetTrigger("StopSummon");
        //Destroy(summonEffect);
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
            print("godie");
            dead = true;
            animator.SetTrigger("Die");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRange, EnemyLayer);                   //Attention! Need to change to AttackPoint
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
