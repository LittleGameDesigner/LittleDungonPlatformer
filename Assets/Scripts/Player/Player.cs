using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //Move
    private float horizontal_direction;
    public int MoveSpeed = 3;
    private float JumpCD = 0.5f;
    //Attack
    public bool isAttacking;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask EnemyLayer;
    public float AttackCD = 1;
    //Material
    private Animator animator;
    public GameObject PlayerSword;
    public GameObject PlayerBow;
    private Color originalColor;
    //Health
    public float Health;
    private float maxHealth = 1;
    public HealthBarBehaviour healthBar;

    void Start()
    {
        animator = GetComponent<Animator>();
        Health = maxHealth;
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
        SwitchToSword(); 
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
        }else if(Input.GetKeyUp(KeyCode.LeftShift)){
            MoveSpeed = 3;
            animator.SetBool("isSprinting", false);
        }

    }

    private void ChangeSword(){
        PlayerSword.SetActive(true);
        PlayerSword ps = PlayerSword.GetComponent<PlayerSword>();
        ps.Health = ps.maxHealth;
        gameObject.SetActive(false);
    }

    private void SwitchToSword(){
        if(Input.GetKeyDown(KeyCode.Alpha2)){
            PlayerSword.SetActive(true);
            PlayerSword ps = PlayerSword.GetComponent<PlayerSword>();
            ps.Health = ps.maxHealth;
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
            Destroy(gameObject);
        }
    }

    private void DemageEffect(){
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", originalColor);
    }
    
    private void DrinkRedPotion(){
        Health += 100;
        healthBar.SetHealth(Health);
    }

    
}
