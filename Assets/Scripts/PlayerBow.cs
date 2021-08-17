using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBow : MonoBehaviour
{
    private float horizontal_direction;
    public int MoveSpeed = 3;
    private float JumpCD = 0.5f;
    private bool isSprinting;
    //Attack
    public bool isAttacking;
    public float AttackCD = 0.8f;
    public float AttackDemage = 10;
    public LayerMask EnemyLayer;
    public GameObject PlayerArrow;
    public Vector3 arrowEulerAngles;
    //Material
    private Animator animator;
    //Health
    public float Health;
    public float maxHealth = 100;
    public HealthBarBehaviour healthBar;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(JumpCD >= 0.5){
            Jump();
        }
        JumpCD += Time.deltaTime;

        Sprint();
        if(AttackCD > 0.8f){
            Attack();
        }else{
            AttackCD += Time.deltaTime; 
        }
    }

    private void FixedUpdate() {
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
            //
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
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 20), ForceMode2D.Impulse);
            JumpCD = 0;
        }
    }

    private void Sprint(){
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            MoveSpeed = 6;
            animator.SetBool("isSprinting", true);
            isSprinting = true;
            //animation["Run"].speed = 2;
        }else if(Input.GetKeyUp(KeyCode.LeftShift)){
            MoveSpeed = 3;
            animator.SetBool("isSprinting", false);
            isSprinting = false;
            //animation["Run"].speed = 1;
        }

    }

    private void Attack(){
        if(Input.GetKeyDown(KeyCode.J) && !isSprinting){
            animator.SetTrigger("Attack");
            Invoke("Shoot", 0.8f);
            AttackCD = 0;
        }
    }

    private void Shoot(){
        Instantiate(PlayerArrow, transform.position, Quaternion.Euler(transform.eulerAngles + arrowEulerAngles));
        FindObjectOfType<AudioManager>().Play("ArrowFly");
    }

    private void TakeDemage(float demage){
        print(demage);
        Health -= demage;
        healthBar.SetHealth(Health);
        if(Health <= 0){
            Destroy(gameObject);
        }
    }
}
