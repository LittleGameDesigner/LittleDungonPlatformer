using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerSword : MonoBehaviour
{
    //Move
    public float horizontal_direction;
    public int MoveSpeed = 3;
    private float JumpCD = 0.5f;
    private bool isSprinting;
    //Attack
    public Transform attackPoint;
    public float attackRange = 0.8f;
    public LayerMask EnemyLayer;
    public float AttackCD = 0.5f;
    private float baseAttackDemage = 10;
    private float AttackDemage;
    public bool canSwitchToBow;
    private bool isAttacking;
    //Material
    [SerializeField] private LayerMask TerrianLayer;
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject PlayerBow;
    private Color originalColor;
    private BoxCollider2D boxCollider2D;
    private GameObject PlayerData;
    //Health
    public float Health;
    public float maxHealth = 100;
    public HealthBarBehaviour healthBar;
    public GameObject Blood;
    private bool dead;
    //EXP
    public int level = 1;
    public float exp = 0;
    public float expGap = 100;
    public float totalEXP;
    public GameObject LevelUp;
    public GameObject LevelUpIcon;
    //Magic
    public float magic;
    public float maxMagic = 50;
    public bool GotFireBall;
    public GameObject FireBall;
    public Vector3 magicEulerAngles;
    //PlayerStat
    public Text lvStat;
    public Text maxHealthStat;
    public Text maxMagicStat;
    public Text attackStat;
    public Text totalEXPStat;


    void Start()
    {
        GotFireBall = true;
        maxHealth = 10000;
        Health = maxHealth;
        maxMagic = 10000;
        magic = maxMagic;
        print(transform.position);
        PlayerData = GameObject.Find("StaticPlayerData");
        //LoadPlayerData();
        AttackDemage = 10;
        animator = GetComponent<Animator>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        healthBar.SetMaxHealth(Health, maxHealth);
        healthBar.SetMaxMagic(magic, maxMagic);
        healthBar.SetEXP(exp, expGap, level);
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
        if((AttackCD > 0.8f) && !isSprinting){
            if(Input.GetKeyDown(KeyCode.J)){
                isAttacking = true;
                animator.SetTrigger("AttackMelee1");
                AttackCD = 0;
                Invoke("AttackMelee1", 0.3f);
            }else{
                CastMagic();
            }
        }
        AttackCD += Time.deltaTime; 
        SwitchToBow();
        AttackDemage = baseAttackDemage * Convert.ToSingle(System.Math.Pow(1.1f, level - 1));
        UpdatePlayerData();
        UpdatePlayerStat();
    }

    private void FixedUpdate() {
        if(dead || isAttacking)return;
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
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded()){
            animator.SetTrigger("Jump");
            FindObjectOfType<AudioManager>().Play("Jump");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 53), ForceMode2D.Impulse);
            JumpCD = 0;
        }
    }

    private bool IsGrounded(){
        RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down, boxCollider2D.bounds.extents.y + 0.01f, TerrianLayer);
        return (raycastHit != null);
    }

    private void Sprint(){
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            MoveSpeed = 6;
            animator.SetBool("isSprinting", true);
            FindObjectOfType<AudioManager>().Play("Walk");
            isSprinting = true;
        }else if(Input.GetKeyUp(KeyCode.LeftShift)){
            MoveSpeed = 3;
            animator.SetBool("isSprinting", false);
            FindObjectOfType<AudioManager>().Stop("Walk");
            isSprinting = false;
        }

    }

    private void AttackMelee1(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            enemy.SendMessage("TakeDemage", AttackDemage, SendMessageOptions.DontRequireReceiver);
            FindObjectOfType<AudioManager>().Play("SwordSwing");
        }
        isAttacking = false;
    }

    private void CastMagic(){
        Vector3 facingDirection = transform.localScale;
        if(facingDirection.x > 0){
            magicEulerAngles = new Vector3(0, 0, 0);
        }else if(facingDirection.x < 0){
            magicEulerAngles = new Vector3(0, 0, -180);
        }
        if(Input.GetKeyDown(KeyCode.Y) && (magic >= 30) && GotFireBall){
            isAttacking = true;
            Invoke("CastFireBall", 0.4f);
            animator.SetTrigger("CastMagic");
            magic -= 30;
            healthBar.SetMagic(magic);
            AttackCD = 0;
        }
    }

    private void CastFireBall(){
        FindObjectOfType<AudioManager>().Play("CastFireBall");
        Instantiate(FireBall, transform.position, Quaternion.Euler(transform.eulerAngles + magicEulerAngles));
        animator.SetTrigger("StopCasting");
        isAttacking = false;
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void SwitchToBow(){
        if(Input.GetKeyDown(KeyCode.Alpha2) && canSwitchToBow){
            PlayerBow.SetActive(true);
            PlayerBow pb = PlayerBow.GetComponent<PlayerBow>();
            pb.Health = Health;
            pb.magic = magic;
            pb.level = level;
            pb.totalEXP = totalEXP;
            pb.exp = exp;
            pb.expGap = expGap;
            pb.maxHealth = maxHealth;
            pb.maxMagic = maxMagic;
            gameObject.SetActive(false);
        }
    }

    private void ChangeSword(){
        return;
    }

    private void ChangeBow(){
        PlayerBow.SetActive(true);
        PlayerBow pb = PlayerBow.GetComponent<PlayerBow>();
        pb.Health = Health;
        pb.magic = magic;
        pb.level = level;
        pb.totalEXP = totalEXP;
        pb.exp = exp;
        pb.expGap = expGap;
        pb.maxHealth = maxHealth;
        pb.maxMagic = maxMagic;
        pb.canSwitchToSword = true;
        canSwitchToBow = true;
        gameObject.SetActive(false);
    }

    private void TakeDemage(float demage){
        if(dead)return;
        Health -= demage;
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", Color.red);
        Instantiate(Blood, transform.position + new Vector3(0, 0.2f, 0), transform.rotation);
        Invoke("DemageEffect", 0.2f);
        healthBar.SetHealth(Health);
        if(Health <= 0){
            dead = true;
            animator.SetTrigger("Die");
            Invoke("Respawn", 1);
        }
    }

    private void PushBack(int d){
        animator.SetTrigger("PushBack");
        rb.velocity = new Vector2(d, rb.velocity.y);
    }

    private void DemageEffect(){
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", originalColor);
    }

    private void GainEXP(int amount){
        exp += amount;
        totalEXP += amount;
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
            maxMagic += 10;
            Health = maxHealth;
            magic = maxMagic;
            healthBar.SetMaxHealth(Health, maxHealth);
            healthBar.SetMaxMagic(magic, maxMagic);
        }
        healthBar.SetEXP(exp, expGap, level);
    }

    private void UpdatePlayerStat(){
        lvStat.text = level.ToString();
        maxHealthStat.text = maxHealth.ToString();
        maxMagicStat.text = maxMagic.ToString();
        attackStat.text = AttackDemage.ToString();
        totalEXPStat.text = totalEXP.ToString();
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

    private void LoadPlayerData(){
        maxMagic = PlayerData.GetComponent<StaticPlayerData>().maxMagic;
        magic = PlayerData.GetComponent<StaticPlayerData>().magic;
        maxHealth = PlayerData.GetComponent<StaticPlayerData>().maxHealth;
        Health = PlayerData.GetComponent<StaticPlayerData>().health;
        level = PlayerData.GetComponent<StaticPlayerData>().level;
        AttackDemage = PlayerData.GetComponent<StaticPlayerData>().AttackDemage;
        exp = PlayerData.GetComponent<StaticPlayerData>().exp;
        expGap = PlayerData.GetComponent<StaticPlayerData>().expGap;
        totalEXP = PlayerData.GetComponent<StaticPlayerData>().totalEXP;
        GotFireBall = PlayerData.GetComponent<StaticPlayerData>().GotFireBall;
        canSwitchToBow = PlayerData.GetComponent<StaticPlayerData>().canSwitchToBow;
        // float x = PlayerData.GetComponent<StaticPlayerData>().positionX;
        // float y = PlayerData.GetComponent<StaticPlayerData>().positionY;
        // float z = PlayerData.GetComponent<StaticPlayerData>().positionZ;
        // transform.position = new Vector3(x, y, z);
        //print(transform.position);
    }

    private void UpdatePlayerData(){
        PlayerData.GetComponent<StaticPlayerData>().maxMagic = maxMagic;
        PlayerData.GetComponent<StaticPlayerData>().magic = magic;
        PlayerData.GetComponent<StaticPlayerData>().maxHealth = maxHealth;
        PlayerData.GetComponent<StaticPlayerData>().health = Health;
        PlayerData.GetComponent<StaticPlayerData>().level = level;
        PlayerData.GetComponent<StaticPlayerData>().AttackDemage = AttackDemage;
        PlayerData.GetComponent<StaticPlayerData>().exp = exp;
        PlayerData.GetComponent<StaticPlayerData>().expGap = expGap;
        PlayerData.GetComponent<StaticPlayerData>().totalEXP = totalEXP;
        PlayerData.GetComponent<StaticPlayerData>().whichPlayer = "PlayerSword";
        // PlayerData.GetComponent<StaticPlayerData>().positionX = transform.position.x;
        // PlayerData.GetComponent<StaticPlayerData>().positionY = transform.position.y;
        // PlayerData.GetComponent<StaticPlayerData>().positionZ = transform.position.z;
    }
}
