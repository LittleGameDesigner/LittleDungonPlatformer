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
    //Health
    public float Health;
    public float maxHealth = 100;
    public HealthBarBehaviour healthBar;



    void Start()
    {
        //gameObject.SetActive(true);
        animator = GetComponent<Animator>();
        Health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        //healthBar.Active(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(JumpCD >= 0.5){
            Jump();
        }
        JumpCD += Time.deltaTime;

        Sprint();
        if(AttackCD > 2){
            AttackMelee1();
            AttackCD = 0;
        }
        AttackCD += Time.deltaTime;    
    }

    private void FixedUpdate() {
        Move();
        //AttackMelee1();
        //Sprint();
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
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
            JumpCD = 0;
        }
    }

    private void Sprint(){
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            MoveSpeed = 6;
            animator.SetBool("isSprinting", true);
            //animation["Run"].speed = 2;
        }else if(Input.GetKeyUp(KeyCode.LeftShift)){
            MoveSpeed = 3;
            animator.SetBool("isSprinting", false);
            //animation["Run"].speed = 1;
        }

    }

    private void AttackMelee1(){
        if(Input.GetKeyDown(KeyCode.J)){
            animator.SetTrigger("AttackMelee1");
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, EnemyLayer);

            foreach (Collider2D enemy in hitEnemies){
                print("we hit");
            }
        }
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    private void ChangeSword(){
        //Instantiate(PlayerSword, transform.position, transform.rotation);
        PlayerSword.SetActive(true);
        gameObject.SetActive(false);
        
        //Destroy(gameObject);
    }

}
