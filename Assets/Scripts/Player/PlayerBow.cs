using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class PlayerBow : MonoBehaviour
{
    public float horizontal_direction;
    public int MoveSpeed = 3;
    private float JumpCD = 0.5f;
    private bool isSprinting;
    //Attack
    private float AttackCD = 1.2f;
    public float AttackCoolDown = 1.2f;
    private float baseAttackDemage = 10;
    public float AttackDemage = 10;
    public LayerMask EnemyLayer;
    public GameObject PlayerArrow;
    public Vector3 arrowEulerAngles;
    public bool canSwitchToSword;
    private bool isAttacking;
    //Material
    [SerializeField] private LayerMask TerrianLayer;
    private Animator animator;
    private Color originalColor;
    public GameObject PlayerSword;
    private BoxCollider2D boxCollider2D;
    //Health
    public float Health;
    public float maxHealth = 100;
    public HealthBarBehaviour healthBar;
    private bool dead;
    //EXP
    public int level = 1;
    public float exp = 0;
    public float expGap = 100;
    public GameObject LevelUp;
    public GameObject LevelUpIcon;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        Health = maxHealth;
        healthBar.SetMaxHealth(Health, maxHealth);
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        originalColor = PlayerRenderrer.material.color;
    }

    void Update()
    {
        if(dead || isAttacking)return;
        if(JumpCD >= 0.5){
            Jump();
        }
        JumpCD += Time.deltaTime;

        Sprint();
        if(AttackCD > AttackCoolDown){
            Attack();
        }else{
            AttackCD += Time.deltaTime; 
        }
        UpdateAttackDemage();
        SwitchToSword();
    }

    private void FixedUpdate() {
        if(dead || isAttacking)return;  
        Move();
    }

    private void Move(){
        horizontal_direction = Input.GetAxisRaw("Horizontal");
        if(horizontal_direction > 0){
            arrowEulerAngles = new Vector3(0, 0, 0);
            Vector3 facingDirection = transform.localScale;
            if(facingDirection.x < 0){
                facingDirection.x *= -1;
            }
            transform.localScale = facingDirection;
            transform.Translate(Vector3.right * MoveSpeed * horizontal_direction * Time.fixedDeltaTime, Space.World);
        }
        
        else if(horizontal_direction < 0){
            arrowEulerAngles = new Vector3(0, 0, -180);
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
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 30), ForceMode2D.Impulse);
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

    private void Attack(){
        if(Input.GetKeyDown(KeyCode.J) && !isSprinting){
            isAttacking = true;
            animator.SetTrigger("Attack");
            Invoke("Shoot", 1);
            AttackCD = 0;
        }
    }

    private void Shoot(){
        Instantiate(PlayerArrow, transform.position, Quaternion.Euler(transform.eulerAngles + arrowEulerAngles));
        FindObjectOfType<AudioManager>().Play("ArrowFly");
        animator.SetTrigger("StopAttack");
        isAttacking = false;
    }

    private void UpdateAttackDemage(){
        AttackDemage = baseAttackDemage * Convert.ToSingle(System.Math.Pow(1.1f, level - 1));
        PlayerArrow pa = PlayerArrow.GetComponent<PlayerArrow>();
        pa.demage = AttackDemage;
    }

    private void SwitchToSword(){
        if(Input.GetKeyDown(KeyCode.Alpha1) && canSwitchToSword){
            PlayerSword.SetActive(true);
            PlayerSword ps = PlayerSword.GetComponent<PlayerSword>();
            ps.Health = Health;
            ps.level = level;
            ps.exp = exp;
            ps.expGap = expGap;
            ps.maxHealth = maxHealth;
            gameObject.SetActive(false);
        }
    }

    private void ChangeSword(){
        PlayerSword.SetActive(true);
        PlayerSword ps = PlayerSword.GetComponent<PlayerSword>();
        ps.Health = Health;
        canSwitchToSword = true;
        ps.canSwitchToBow = true;
        gameObject.SetActive(false);
    }

    private void ChangeBow(){
        return;
    }

    private void TakeDemage(float demage){
        if(dead)return;
        Health -= demage;
        healthBar.SetHealth(Health);
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", Color.red);
        Invoke("DemageEffect", 0.2f);
        if(Health <= 0){
            animator.SetTrigger("Die");
            dead = true;
            Invoke("Respawn", 1);
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

    private void Respawn(){
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
}
