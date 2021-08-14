using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSword : MonoBehaviour
{
    //Move
    private float horizontal_direction;
    public int MoveSpeed = 3;
    private float JumpCD = 0.5f;
    private bool isSprinting;

    //Attack
    public bool isAttacking;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask EnemyLayer;

    //Animation
    private Animator animator;


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(JumpCD >= 0.5){
            Jump();
        }
        JumpCD += Time.deltaTime;

        Sprint();
        AttackMelee1();
        
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
            isSprinting = true;
            //animation["Run"].speed = 2;
        }else if(Input.GetKeyUp(KeyCode.LeftShift)){
            MoveSpeed = 3;
            animator.SetBool("isSprinting", false);
            isSprinting = false;
            //animation["Run"].speed = 1;
        }

    }

    private void AttackMelee1(){
        if(Input.GetKeyDown(KeyCode.J) && !isSprinting){
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

}
