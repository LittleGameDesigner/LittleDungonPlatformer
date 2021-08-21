using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerSword : MonoBehaviour
{
    //Move
    public float horizontal_direction;
    public int MoveSpeed = 3;
    private float JumpCD = 0.5f;
    private bool isSprinting;
    //Attack
    public bool isAttacking;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask EnemyLayer;
    public float AttackCD = 0.5f;
    private float baseAttackDemage = 10;
    private float AttackDemage;
    //Material
    private Animator animator;
    public GameObject PlayerBow;
    private Color originalColor;
    //Health
    public float Health;
    public float maxHealth = 100;
    public HealthBarBehaviour healthBar;
    //EXP
    public int level = 1;
    public float exp = 0;
    public float expGap = 100;
    public GameObject LevelUp;
    public GameObject LevelUpIcon;


    void Start()
    {
        AttackDemage = 10;
        animator = GetComponent<Animator>();
        healthBar.SetMaxHealth(Health, maxHealth);
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        originalColor = PlayerRenderrer.material.color;
    }

    void Update()
    {   
        if(JumpCD >= 0.5){
            Jump();
        }
        JumpCD += Time.deltaTime;

        Sprint();
        if(AttackCD > 0.8f){
            if(Input.GetKeyDown(KeyCode.J) && !isSprinting){
                animator.SetTrigger("AttackMelee1");
                AttackCD = 0;
                Invoke("AttackMelee1", 0.3f);
            }
        }
        AttackCD += Time.deltaTime; 
        SwitchToBow();
        AttackDemage = baseAttackDemage * Convert.ToSingle(System.Math.Pow(1.1f, level - 1));        
    }

    private void FixedUpdate() {
        Move();
    }

    private void Move(){
        horizontal_direction = Input.GetAxisRaw("Horizontal");
        if(horizontal_direction > 0){
            Vector3 facingDirection = transform.localScale;
            if(facingDirection.x < 0){
                facingDirection.x *= -1;
            }
            transform.localScale = facingDirection;
            transform.Translate(Vector3.right * MoveSpeed * horizontal_direction * Time.fixedDeltaTime, Space.World);
            //
        }
        
        else if(horizontal_direction < 0){
            Vector3 facingDirection = transform.localScale;
            if(facingDirection.x > 0){
                facingDirection.x *= -1;
            }
            transform.localScale = facingDirection;
            transform.Translate(Vector3.right * MoveSpeed * horizontal_direction * Time.fixedDeltaTime, Space.World);
        }

        animator.SetBool("isRunning", true);
        if(horizontal_direction == 0){
            animator.SetBool("isRunning", false);
        }
    }

    private void Jump(){
        if(Input.GetKeyDown(KeyCode.Space)){
            animator.SetTrigger("Jump");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 33), ForceMode2D.Impulse);
            JumpCD = 0;
        }
    }

    private void Sprint(){
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            MoveSpeed = 6;
            animator.SetBool("isSprinting", true);
            isSprinting = true;
        }else if(Input.GetKeyUp(KeyCode.LeftShift)){
            MoveSpeed = 3;
            animator.SetBool("isSprinting", false);
            isSprinting = false;
        }

    }

    private void AttackMelee1(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            enemy.SendMessage("TakeDemage", AttackDemage, SendMessageOptions.DontRequireReceiver);
            FindObjectOfType<AudioManager>().Play("SwordSwing");
        }
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void SwitchToBow(){
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            PlayerBow.SetActive(true);
            PlayerBow pb = PlayerBow.GetComponent<PlayerBow>();
            pb.Health = Health;
            pb.level = level;
            pb.exp = exp;
            pb.expGap = expGap;
            pb.maxHealth = maxHealth;
            gameObject.SetActive(false);
        }
    }

    private void TakeDemage(float demage){
        Health -= demage;
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", Color.red);
        Invoke("DemageEffect", 0.2f);
        healthBar.SetHealth(Health);
        if(Health <= 0){
            animator.SetBool("Die", true);
            Destroy(gameObject, 1);
        }
    }

    private void DemageEffect(){
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", originalColor);
    }

    private void GainEXP(int amount){
        exp += amount;
        while(exp >= expGap){
            FindObjectOfType<AudioManager>().Play("LevelUp");
            Instantiate(LevelUp, transform.position, transform.rotation);
            var icon = Instantiate(LevelUpIcon, transform.position + new Vector3(0, 1.5f, 0), transform.rotation);
            Destroy(icon, 0.5f);
            exp -= expGap;
            level += 1;
            expGap *= 1.2f;
            AttackDemage *= 1.1f;
            maxHealth += 30;
            Health = maxHealth;
            healthBar.SetMaxHealth(Health, maxHealth);
        }
    }

    private void DrinkRedPotion(){
        Health += 100;
        if(Health > maxHealth){Health = maxHealth;}
        healthBar.SetHealth(Health);
    }

    private void DrinkYellowPotion(){
        GainEXP(100);
    }

}
