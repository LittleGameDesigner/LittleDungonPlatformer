using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{
    //Form
    public bool inSwordForm;
    public bool inBowForm;
    public bool needLoad;
    //Move
    private float horizontal_direction;
    private int MoveSpeed = 3;
    private float JumpCD = 0.5f;
    private bool isSprinting;
    private int sprintSpeed = 6;
    //Attack
    public Transform attackPoint;
    private float attackRange = 0.8f;
    private Vector3 arrowEulerAngles;
    public LayerMask EnemyLayer;
    private float AttackCD = 0.5f;
    private float attackCoolDown = 0.8f;
    private float rangedCoolDown = 1.2f;
    private float baseAttackDemage = 10;
    public float AttackDemage;
    public bool canSwitchToBow;
    public bool canSwitchToSword;
    private bool isAttacking;
    //Material
    [SerializeField] private LayerMask TerrianLayer;
    private Rigidbody2D rb;
    private Animator animator;
    public GameObject PlayerArrow;
    public GameObject PlayerStat;
    public GameObject Setting;
    private Color originalColor;
    private BoxCollider2D boxCollider2D;
    private GameObject PlayerData;
    //Health
    public float health;
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
    private float MagicCD;
    private float magicCoolDown = 0.8f;
    private Vector3 magicEulerAngles;
    //MagicalSkills
    public bool GotFireBall;
    public GameObject FireBall;
    private float FireBallManaCost = 30;
    private bool BerserkerRageOn;
    public bool GotBerserkerRage;
    private float BerserkerRageDuration;
    public GameObject BerserkerRageEffect;
    public GameObject BerserkerRageRoar;
    private float BerserkerRageManaCost = 5;
    public bool GotTornado;
    public GameObject Tornado;
    private float TornadoManaCost = 45;
    public bool GotWaterSplash;
    public GameObject WaterSplash;
    private float WaterSplashManaCost = 30;
    //PlayerStat
    public Text lvStat;
    public Text maxHealthStat;
    public Text maxMagicStat;
    public Text attackStat;
    public Text totalEXPStat;

    void Awake()
    {

    }

    void Start()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")return;
        AttackDemage = 10;
        animator = GetComponent<Animator>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        health = maxHealth;
        magic = maxMagic;
        PlayerData data = SaveManager.Load(this);
        ReadLoadData(data);
        healthBar.SetMaxHealth(health, maxHealth);
        healthBar.SetMaxMagic(magic, maxMagic);
        healthBar.SetEXP(exp, expGap, level);
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        originalColor = PlayerRenderrer.material.color;
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name == "MainMenu")return;
        if(dead || isAttacking)return;
        if(inSwordForm){
            if((AttackCD > attackCoolDown) && !isSprinting){
                if(Input.GetKeyDown(KeyCode.J)){
                    isAttacking = true;
                    animator.SetTrigger("Attack");
                    AttackCD = 0;
                    Invoke("AttackMelee1", 0.3f);
                }
            }
            if(MagicCD > magicCoolDown){
                CastMagic();
            }
        }else if(inBowForm){
            if((AttackCD > rangedCoolDown) && !isSprinting){
                if(Input.GetKeyDown(KeyCode.J)){
                    AttackRanged1();
                }
            }
            if(MagicCD > magicCoolDown){
                CastMagic();
            }

            if(BerserkerRageOn){
                BerserkerRageDuration -= Time.deltaTime;
                if(BerserkerRageDuration <= 0){
                    BerserkerRageOn = false;
                    MoveSpeed = 3;
                    sprintSpeed = 6;
                    rangedCoolDown = 1.2f;
                    animator.speed = 1;
                    var PlayerRenderrer = gameObject.GetComponent<Renderer>();
                    PlayerRenderrer.material.SetColor("_Color", originalColor);
                    BerserkerRageEffect.SetActive(false);
                }
            }
        }
        Sprint();
        SwitchWeapon();
        if(JumpCD >= 0.5){
            Jump();
        }
        JumpCD += Time.deltaTime;
        AttackCD += Time.deltaTime;
        MagicCD += Time.deltaTime;
        AttackDemage = baseAttackDemage * Convert.ToSingle(System.Math.Pow(1.1f, level - 1));
        UpdateAttackDemage();
        UpdatePlayerStat();
        OpenMenu();
    }

    private void FixedUpdate() {
        if(dead || isAttacking || SceneManager.GetActiveScene().name == "MainMenu")return;
        Move();
    }

    private void OpenMenu(){
        if(Input.GetKeyDown(KeyCode.Escape) && !Setting.activeSelf){
            if(PlayerStat.activeSelf){
                PlayerStat.SetActive(false);
            }else{
                PlayerStat.SetActive(true);
            }
        }
    }

    public void ReadLoadData(PlayerData data){
        var playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        playerController.transform.position = new Vector3(data.positionX, data.positionY, data.positionZ);
        playerController.maxMagic = data.maxMagic;
        playerController.magic = data.magic;
        playerController.maxHealth = data.maxHealth;
        playerController.health = data.health;
        playerController.level = data.level;
        playerController.AttackDemage = data.AttackDemage;
        playerController.exp = data.exp;
        playerController.expGap = data.expGap;
        playerController.totalEXP = data.totalEXP;
        playerController.GotFireBall = data.GotFireBall;
        playerController.GotBerserkerRage = data.GotBerserkerRage;
        playerController.canSwitchToBow = data.canSwitchToBow;
        playerController.canSwitchToSword = data.canSwitchToSword;
    }

    private void UpdatePlayerStat(){
        lvStat.text = level.ToString();
        maxHealthStat.text = maxHealth.ToString();
        maxMagicStat.text = maxMagic.ToString();
        attackStat.text = AttackDemage.ToString();
        totalEXPStat.text = totalEXP.ToString();
    }

    private void SwitchWeapon(){
        if(BerserkerRageOn)return;
        if(Input.GetKeyDown(KeyCode.Alpha1) && canSwitchToSword){
            ChangeSword();
        }else if(Input.GetKeyDown(KeyCode.Alpha2) && canSwitchToBow){
            ChangeBow();
        }
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
            if(isSprinting){
                transform.Translate(Vector3.right * sprintSpeed * horizontal_direction * Time.fixedDeltaTime, Space.World);
            }else{
                transform.Translate(Vector3.right * MoveSpeed * horizontal_direction * Time.fixedDeltaTime, Space.World);
            }
        }
        
        else if(horizontal_direction < 0){
            arrowEulerAngles = new Vector3(0, 0, -180);
            Vector3 facingDirection = transform.localScale;
            if(facingDirection.x > 0){
                facingDirection.x *= -1;
            }
            transform.localScale = facingDirection;
            if(isSprinting){
                transform.Translate(Vector3.right * sprintSpeed * horizontal_direction * Time.fixedDeltaTime, Space.World);
            }else{
                transform.Translate(Vector3.right * MoveSpeed * horizontal_direction * Time.fixedDeltaTime, Space.World);
            }
        }

        animator.SetBool("isRunning", true);
        if(horizontal_direction == 0){
            animator.SetBool("isRunning", false);
        }
    }

    private void Sprint(){
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            animator.SetBool("isSprinting", true);
            FindObjectOfType<AudioManager>().Play("Walk");
            isSprinting = true;
        }else if(Input.GetKeyUp(KeyCode.LeftShift)){
            animator.SetBool("isSprinting", false);
            FindObjectOfType<AudioManager>().Stop("Walk");
            isSprinting = false;
        }

    }

    private void Jump(){
        if(Input.GetKeyDown(KeyCode.Space) && IsGrounded()){
            animator.SetTrigger("Jump");
            FindObjectOfType<AudioManager>().Play("Jump");
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 33), ForceMode2D.Impulse);
            JumpCD = 0;
        }
    }

    private bool IsGrounded(){
        RaycastHit2D raycastHit = Physics2D.Raycast(boxCollider2D.bounds.center, Vector2.down, boxCollider2D.bounds.extents.y + 0.01f, TerrianLayer);
        return (raycastHit != null);
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
            health = maxHealth;
            magic = maxMagic;
            healthBar.SetMaxHealth(health, maxHealth);
            healthBar.SetMaxMagic(magic, maxMagic);
        }
        healthBar.SetEXP(exp, expGap, level);
    }

    private void TakeDemage(float demage){
        if(dead)return;
        if(BerserkerRageOn){
            FindObjectOfType<AudioManager>().Play("Block");
            return;
        }
        health -= demage;
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", Color.red);
        Instantiate(Blood, transform.position + new Vector3(0, 0.2f, 0), transform.rotation);
        Invoke("DemageEffect", 0.2f);
        healthBar.SetHealth(health);
        if(health <= 0){
            dead = true;
            animator.SetTrigger("Die");
            Invoke("Respawn", 1);
        }
    }

    private void DemageEffect(){
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", originalColor);
    }

    private void PushBack(int d){
        animator.SetTrigger("PushBack");
        rb.velocity = new Vector2(d, rb.velocity.y);
    }

    private void DrinkRedPotion(){
        health += 100;
        if(health > maxHealth){health = maxHealth;}
        healthBar.SetHealth(health);
    }

    private void DrinkYellowPotion(){
        GainEXP(100);
    }

    private void Respawn(){
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }

    private void AttackMelee1(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            enemy.SendMessage("TakeDemage", AttackDemage, SendMessageOptions.DontRequireReceiver);
            FindObjectOfType<AudioManager>().Play("SwordSwing");
        }
        isAttacking = false;
        animator.SetTrigger("StopAttack");
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void CastMagic(){
        Vector3 facingDirection = transform.localScale;
        if(facingDirection.x > 0){
            magicEulerAngles = new Vector3(0, 0, 0);
        }else if(facingDirection.x < 0){
            magicEulerAngles = new Vector3(0, 0, -180);
        }

        if(Input.GetKeyDown(KeyCode.Y) && (magic >= FireBallManaCost) && GotFireBall && inSwordForm){
            isAttacking = true;
            Invoke("CastFireBall", 0.4f);
            animator.SetTrigger("CastMagic");
            magic -= FireBallManaCost;
            healthBar.SetMagic(magic);
            MagicCD = 0;
        }else if(Input.GetKeyDown(KeyCode.T) && (magic >= TornadoManaCost) && GotTornado && inSwordForm){
            isAttacking = true;
            Invoke("CastTornado", 0.4f);
            animator.SetTrigger("CastMagic");
            magic -= TornadoManaCost;
            healthBar.SetMagic(magic);
            MagicCD = 0;
        }else if(Input.GetKeyDown(KeyCode.U) && (magic >= WaterSplashManaCost) && GotWaterSplash && inSwordForm){
            isAttacking = true;
            Invoke("CastWaterSplash", 0.4f);
            animator.SetTrigger("CastMagic");
            magic -= WaterSplashManaCost;
            healthBar.SetMagic(magic);
            MagicCD = 0;
        }

        if(Input.GetKeyDown(KeyCode.P) && GotBerserkerRage && inBowForm){
            if(magic >= BerserkerRageManaCost){
                BerserkerRage();
                magic -= BerserkerRageManaCost;
                healthBar.SetMagic(magic);
                MagicCD = 0;
            }else{
                print("not enough magic");
            }
        }
    }

    private void CastFireBall(){
        FindObjectOfType<AudioManager>().Play("CastFireBall");
        Instantiate(FireBall, transform.position, Quaternion.Euler(transform.eulerAngles + magicEulerAngles));
        animator.SetTrigger("StopCasting");
        isAttacking = false;
    }

    private void CastTornado(){
        Instantiate(Tornado, transform.position + new Vector3(5 * transform.localScale.x, 0, 0), Tornado.transform.rotation);
        animator.SetTrigger("StopCasting");
        isAttacking = false;
    }

    private void CastWaterSplash(){
        Instantiate(WaterSplash, transform.position + new Vector3(5 * transform.localScale.x, 0, 0), Tornado.transform.rotation);
        animator.SetTrigger("StopCasting");
        isAttacking = false;
    }

    private void AttackRanged1(){
        if(Input.GetKeyDown(KeyCode.J) && !isSprinting){
            isAttacking = true;
            animator.SetTrigger("Attack");
            if(BerserkerRageOn){
                Invoke("Shoot", 0.5f);
            }else{
                Invoke("Shoot", 1);
            }
            AttackCD = 0;
        }
    }

    private void Shoot(){
        Instantiate(PlayerArrow, transform.position, Quaternion.Euler(transform.eulerAngles + arrowEulerAngles));
        if(BerserkerRageOn){
            Instantiate(PlayerArrow, transform.position + new Vector3(0, 0.5f, 0), Quaternion.Euler(transform.eulerAngles + arrowEulerAngles));
            Instantiate(PlayerArrow, transform.position + new Vector3(0, -0.5f, 0), Quaternion.Euler(transform.eulerAngles + arrowEulerAngles));
        }
        FindObjectOfType<AudioManager>().Play("ArrowFly");
        animator.SetTrigger("StopAttack");
        isAttacking = false;
    }

    private void BerserkerRage(){
        BerserkerRageOn = true;
        FindObjectOfType<AudioManager>().Play("BerserkerRage");
        Instantiate(BerserkerRageRoar, transform.position, transform.rotation);
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", Color.red);
        BerserkerRageDuration = 4;
        MoveSpeed = 6;
        sprintSpeed = 12;
        rangedCoolDown = 0.6f;
        animator.speed = 2;
        BerserkerRageEffect.SetActive(true);
    }

    private void UpdateAttackDemage(){
        AttackDemage = baseAttackDemage * Convert.ToSingle(System.Math.Pow(1.1f, level - 1));
        PlayerArrow pa = PlayerArrow.GetComponent<PlayerArrow>();
        pa.demage = AttackDemage;
    }

    public void ChangeSword(){
        animator.SetTrigger("switchToSword");
        inSwordForm = true;
        inBowForm = false;
        canSwitchToSword = true;
    }

    public void ChangeBow(){
        animator.SetTrigger("switchToBow");
        inBowForm = true;
        inSwordForm = false;
        canSwitchToBow = true;
    }    
}
