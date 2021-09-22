using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DJBoss : MonoBehaviour
{
    //Material
    public Transform attackPoint;
    public LayerMask EnemyLayer;
    private GameObject Player;
    public GameObject electricBall;
    public GameObject chargingFire;
    private Rigidbody2D rb;
    private Animator animator;
    public Color orange;
    public BossDoor BossDoor1;
    public BossDoor BossDoor2;
    System.Random rnd = new System.Random();
    //Move
    private float moveSpeed = 2;
    private bool findPlayer;
    private bool finishedRangeAttack;
    //Attack
    private float skillCD = 10;
    public GameObject kickEffect;
    private bool isJumping;
    private bool isAttacking;
    private bool onPlatform;
    private bool inMiddle;
    private bool onChargingPosition;
    private bool readyForNewSkill = true;
    private bool PlayChargeBGM;
    private float chargingFireWait = 2;
    private int direction;
    private float attackCD;
    private float attackCoolDown = 2;
    private int skills;
    //Health
    public float health;
    public float maxHealth = 500;
    public HealthBarBehaviour healthBar;
    private bool dead;
    private Color originalColor;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        originalColor = PlayerRenderrer.material.color;
        health = maxHealth;        
    }

    void Update()
    {
        skills = rnd.Next(1, 7);
        if((XRange(Player) < 20) && (YRange(Player) < 15) && !findPlayer){
            animator.SetTrigger("PlayerInRange");
            if(!findPlayer){
                BossDoor1.closeDoor = true;
                BossDoor2.closeDoor = true;
                FindObjectOfType<AudioManager>().Stop("Chapter2Theme");
                FindObjectOfType<AudioManager>().Play("DJBossTheme");
                healthBar.SetMaxHealth(health, maxHealth);
                healthBar.Active(true);
                findPlayer = true;
            }
        }else if(((XRange(Player) > 20) || (YRange(Player) > 15)) && !findPlayer){
            return;
        }
        if(skillCD <= 0){
            if((skills <= 3) && readyForNewSkill){
                isJumping = true;
                skillCD = 20;
            }else{
                readyForNewSkill = false;
                isAttacking = true;
                chargingFire.SetActive(true);
                if(!PlayChargeBGM){
                    FindObjectOfType<AudioManager>().Play("Charging");
                    animator.SetTrigger("Charging");
                    var PlayerRenderrer = gameObject.GetComponent<Renderer>();
                    PlayerRenderrer.material.SetColor("_Color", orange);
                    PlayChargeBGM = true;
                }
                if(chargingFireWait >= 0){
                    chargingFireWait -= Time.deltaTime;
                    return;
                }
                Invoke("ChargingFireRight", 0.5f);
                Invoke("ChargingFireLeft", 0.5f);
                //skillCD = 20;
            }
            return;
        }
        if(isJumping){
            Jump();
            return;
        }
        if(onPlatform){
            if(!inMiddle && !finishedRangeAttack){
                WalkToMiddle();
            }else if(inMiddle && !finishedRangeAttack){
                isAttacking = true;
                RangeAttack();
                inMiddle = false;
                finishedRangeAttack = true;
            }else{
                if(!isAttacking){WalkDownPlatform();}
            }
            return;
        }

        ChangeDirection("player");
        if(PlayerInAttackRange()){
            transform.Translate(Vector3.right * 0, Space.World);
            if(attackCD > attackCoolDown){
                isAttacking = true;
                animator.SetTrigger("Attack");
                Invoke("Attack", 0.7f);
                attackCD = 0;
            }
        }else{
            if(!isAttacking)Chase();
        }

        attackCD += Time.deltaTime;
        skillCD -= Time.deltaTime;

        
    }

    private void Attack(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 3, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            enemy.SendMessage("PushBack", direction * 30, SendMessageOptions.DontRequireReceiver);
            enemy.SendMessage("TakeDemage", 20, SendMessageOptions.DontRequireReceiver);
            FindObjectOfType<AudioManager>().Play("MachineKick");
            Instantiate(kickEffect, Player.transform.position, Player.transform.rotation);
        }
        //timeSinceLastAttack = Time.time;
        animator.SetTrigger("StopAttack");
        isAttacking = false;
    }

    private void OnDrawGizmosSelected(){
        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, 3);
    }

    private void ChangeDirection(string d){
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        Vector3 facingDirection = transform.localScale;
        if(d == "player"){
            if(Player.transform.position.x > transform.position.x){
                if(facingDirection.x > 0){
                    facingDirection.x *= -1;
                }
                direction = 1;
            }else{
                if(facingDirection.x < 0){
                    facingDirection.x *= -1;
                }
                direction = -1;
            }
        }else if(d == "left"){
            if(facingDirection.x < 0){
                facingDirection.x *= -1;
            }
        }else if(d == "right"){
            if(facingDirection.x > 0){
                facingDirection.x *= -1;
            }
        }
        transform.localScale = facingDirection;
    }

    private void Chase(){
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(Player.transform.position.x > transform.position.x){
            transform.Translate(Vector3.right * moveSpeed * 1 * Time.deltaTime, Space.World);
        }else if(Player.transform.position.x < transform.position.x){
            transform.Translate(Vector3.right * moveSpeed * -1 * Time.deltaTime, Space.World);
        }
    }

    private void Jump(){
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(Player.transform.position.x < transform.position.x){
            JumpToPlatformRightSide();
        }else{
            JumpToPlatformLeftSide();
        }
    }

    private void JumpToPlatformRightSide(){
        if(transform.position.x < 117.5f){
            animator.speed = 3;
            ChangeDirection("right");
            transform.Translate(Vector3.right * moveSpeed * 3 * Time.deltaTime, Space.World);
            return;
        }
        animator.speed = 1;
        animator.SetTrigger("Jump");
        rb.AddForce(new Vector2(0, 50), ForceMode2D.Impulse);
        rb.velocity = new Vector2(-15, rb.velocity.y);
        isJumping = false;
        onPlatform = true;
    }

    private void JumpToPlatformLeftSide(){
        if(transform.position.x > 85){
            animator.speed = 3;
            ChangeDirection("left");
            transform.Translate(Vector3.right * moveSpeed * -3 * Time.deltaTime, Space.World);
            return;
        }
        animator.speed = 1;
        animator.SetTrigger("Jump");
        rb.AddForce(new Vector2(0, 50), ForceMode2D.Impulse);
        rb.velocity = new Vector2(15, rb.velocity.y);
        isJumping = false;
        onPlatform = true;
    }

    private void WalkToMiddle(){
        if(transform.position.x > 110){
            ChangeDirection("left");
            transform.Translate(Vector3.right * moveSpeed * -1 * Time.deltaTime, Space.World);
            return;
        }else if(transform.position.x < 93){
            ChangeDirection("right");
            transform.Translate(Vector3.right * moveSpeed * 1 * Time.deltaTime, Space.World);
            return;
        }
        inMiddle = true;
    }

    private void WalkDownPlatform(){
        if(transform.position.x < 98){
            ChangeDirection("right");
            transform.Translate(Vector3.right * moveSpeed * 1 * Time.deltaTime, Space.World);
            return;
        }else if(transform.position.x > 105){
            ChangeDirection("left");
            transform.Translate(Vector3.right * moveSpeed * -1 * Time.deltaTime, Space.World);
            return;
        }
        onPlatform = false;
        finishedRangeAttack = false;
    }

    private void RangeAttack(){
        
        StartCoroutine(RangeAttackCoroutine());
    }

    IEnumerator RangeAttackCoroutine(){
        FindObjectOfType<AudioManager>().Play("DJRangeAttack");
        yield return new WaitForSeconds(0.1f);
        animator.SetTrigger("RangeAttack");
        Instantiate(electricBall, transform.position + new Vector3(1, 0, 0), transform.rotation);
        yield return new WaitForSeconds(0.2f);
        Instantiate(electricBall, transform.position + new Vector3(0.75f, -0.75f, 0), transform.rotation);
        yield return new WaitForSeconds(0.2f);
        Instantiate(electricBall, transform.position + new Vector3(0, -1, 0), transform.rotation);
        yield return new WaitForSeconds(0.2f);
        Instantiate(electricBall, transform.position + new Vector3(-0.75f, -0.75f, 0), transform.rotation);
        yield return new WaitForSeconds(0.2f);
        Instantiate(electricBall, transform.position + new Vector3(-1, 0, 0), transform.rotation);
        yield return new WaitForSeconds(0.2f);
        Instantiate(electricBall, transform.position + new Vector3(-0.75f, 0.75f, 0), transform.rotation);
        yield return new WaitForSeconds(0.2f);
        Instantiate(electricBall, transform.position + new Vector3(0, 1, 0), transform.rotation);
        yield return new WaitForSeconds(0.2f);
        Instantiate(electricBall, transform.position + new Vector3(0.75f, 0.75f, 0), transform.rotation);
        yield return new WaitForSeconds(0.2f);
        isAttacking = false;
    }

    private void ChargingFireLeft(){
        if(!onChargingPosition)return;
        if(transform.position.x > 82){
            ChangeDirection("left");
            transform.Translate(Vector3.right * 20 * -1 * Time.deltaTime, Space.World);
            return;
        }
        isAttacking = false;
        onChargingPosition = false;
        chargingFire.SetActive(false);
        skillCD = 20;
        chargingFireWait = 2;
        PlayChargeBGM = false;
        readyForNewSkill = true;
        animator.SetTrigger("StopCharging");
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", originalColor);
    }

    private void ChargingFireRight(){
        if(onChargingPosition)return;
        if(transform.position.x < 117){
            ChangeDirection("right");
            transform.Translate(Vector3.right * 20 * 1 * Time.deltaTime, Space.World);
            return;
        }
        onChargingPosition = true;
    }

    private bool PlayerInAttackRange(){
        if((XRange(Player) < 3) && (YRange(Player) < 3)){
            return true;
        }
        return false;
    }

    private void TakeDemage(float demage){
        if(dead)return;
        health -= demage;
        healthBar.Active(true);
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", Color.red);
        Invoke("DemageEffect", 0.2f);
        healthBar.SetHealth(health);
        if(health <= 0){
            dead = true;
            animator.SetTrigger("Die");
            FindObjectOfType<AudioManager>().Stop("DJBossTheme");
            FindObjectOfType<AudioManager>().Play("Chapter2Theme");
            BossDoor1.openDoor = true;
                BossDoor2.openDoor = true;
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 100, EnemyLayer);
            foreach (Collider2D enemy in hitEnemies){
                enemy.SendMessage("GainEXP", 500, SendMessageOptions.DontRequireReceiver);
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

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
    }
}
