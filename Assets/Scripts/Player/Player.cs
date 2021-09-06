using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    //Move
    private float horizontal_direction;
    public int MoveSpeed = 3;
    private float JumpCD = 0.5f;
    //Attack
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask EnemyLayer;
    public float AttackCD = 1;
    //Material
    [SerializeField] private LayerMask TerrianLayer; 
    private Animator animator;
    public GameObject PlayerSword;
    public GameObject PlayerBow;
    private Color originalColor;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rb;
    //Health
    public float Health;
    private float maxHealth = 1;
    public HealthBarBehaviour healthBar;
    public GameObject Blood;
    private bool dead;

    void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = transform.GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        Health = maxHealth;
        healthBar.SetMaxHealth(Health, maxHealth);
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        originalColor = PlayerRenderrer.material.color;
    }


    void Update()
    {
        if(dead)return;
        if(JumpCD >= 0.5){
            Jump();
        }
        JumpCD += Time.deltaTime;
        Sprint();
    }

    private void FixedUpdate() {
        if(dead)return;
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
            GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 33), ForceMode2D.Impulse);
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
            FindObjectOfType<AudioManager>().Play("Walk");
            animator.SetBool("isSprinting", true);
        }else if(Input.GetKeyUp(KeyCode.LeftShift)){
            MoveSpeed = 3;
            FindObjectOfType<AudioManager>().Stop("Walk");
            animator.SetBool("isSprinting", false);
        }

    }

    private void ChangeSword(){
        PlayerSword.SetActive(true);
        PlayerSword ps = PlayerSword.GetComponent<PlayerSword>();
        ps.Health = ps.maxHealth;
        ps.magic = ps.maxMagic;
        PlayerBow pb = PlayerBow.GetComponent<PlayerBow>();
        pb.canSwitchToSword = true;
        gameObject.SetActive(false);
    }

    private void ChangeBow(){
        PlayerBow.SetActive(true);
        PlayerBow pb = PlayerBow.GetComponent<PlayerBow>();
        pb.Health = pb.maxHealth;
        pb.magic = pb.maxMagic;
        PlayerSword ps = PlayerSword.GetComponent<PlayerSword>();
        ps.canSwitchToBow = true;
        gameObject.SetActive(false);
    }

    private void TakeDemage(float demage){
        if(dead)return;
        Health -= demage;
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", Color.red);
        Invoke("DemageEffect", 0.2f);
        Instantiate(Blood, transform.position + new Vector3(0, 0.2f, 0), transform.rotation);
        healthBar.SetHealth(Health);
        if(Health <= 0){
            dead = true;
            animator.SetBool("Die", true);
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
    
    private void DrinkRedPotion(){return;}

    private void DrinkYellowPotion(){return;}

    private void Respawn(){
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);
    }
    
}
