using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoonBoss : MonoBehaviour
{
    //Move
    private bool playerInRange;
    private float moveSpeed = 5;
    private bool chased;
    //Material
    System.Random rnd = new System.Random();
    public Transform attackPoint;
    public LayerMask EnemyLayer;
    private GameObject Player;
    private Animator animator;
    //Health
    public float health;
    public float maxHealth = 500;
    public HealthBarBehaviour healthBar;
    private bool dead;
    private Color originalColor;
    //Attack
    private bool isAttacking;
    private float attackCD;
    private float attackCoolDown = 5;
    private float direction;
    private Vector3 eulerAngles;
    //Magic
    private float magicCD = 5;
    private float magicCoolDown = 8;
    private int whichMagic;
    public GameObject CrescentMoon;
    public GameObject Tornado;
    public GameObject LightningSpear;
    public GameObject LightningCloud;
    public GameObject RedLotus;
    private bool useB52;
    private bool onB52Pos;
    private bool diveB52;
    private float ratio;
    private float B52Rate = 636;
    public GameObject B52Bomb;
    public GameObject ChargingFire;

    void Start()
    {
        animator = GetComponent<Animator>();
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    void Update()
    {
        if((XRange(Player) < 36) && (YRange(Player) < 15))playerInRange = true;
        if(dead||isAttacking||!playerInRange)return;

        if(useB52){
            GoToB52Pos();
            return;
        }

        animator.SetTrigger("StopB52");

        ChangeDirection("player");

        if(magicCD > magicCoolDown){
            isAttacking = true;
            animator.SetTrigger("CastMagic");
            Invoke("CastMagic", 1.5f);
            magicCD = 0;
        }

        if((attackCD > attackCoolDown) && (XRange(Player) < 4) && (YRange(Player) < 5)){
            isAttacking = true;
            Invoke("TailAttack", 0.8f);
            animator.SetTrigger("TailAttack");
            attackCD = 0;
        }
        Chase();
        magicCD += Time.deltaTime;
        attackCD += Time.deltaTime;
    }

    private void Chase(){
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if(Math.Abs(Player.transform.position.x - transform.position.x) > 15)chased = false;
        if(Math.Abs(Player.transform.position.x - transform.position.x) < 15 && chased)return;
        if(Player.transform.position.x > transform.position.x + 7){
            transform.Translate(Vector3.right * moveSpeed * 1 * Time.deltaTime, Space.World);
        }else if(Player.transform.position.x < transform.position.x - 7){
            transform.Translate(Vector3.right * moveSpeed * -1 * Time.deltaTime, Space.World);
        }
        if(Math.Abs(Player.transform.position.x - transform.position.x) <= 7)chased = true;
    }

    private void ChangeDirection(string d){
        Vector3 facingDirection = transform.localScale;
        if(d == "player"){
            if(Player.transform.position.x < transform.position.x){
                if(facingDirection.x > 0){
                    facingDirection.x *= -1;
                }
                direction = -1;
                eulerAngles = new Vector3(0, 0, -180);
            }else{
                if(facingDirection.x < 0){
                    facingDirection.x *= -1;
                }
                direction = 1;
                eulerAngles = new Vector3(0, 0, 0);
            }
        }else if(d == "left"){
            if(facingDirection.x > 0){
                facingDirection.x *= -1;
                eulerAngles = new Vector3(0, 0, -180);
            }
        }else if(d == "right"){
            if(facingDirection.x < 0){
                facingDirection.x *= -1;
                eulerAngles = new Vector3(0, 0, 0);
            }
        }
        transform.localScale = facingDirection;
    }

    private void TailAttack(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, 1, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            print("hit");
            enemy.SendMessage("PushBack", direction * 30, SendMessageOptions.DontRequireReceiver);
            enemy.SendMessage("TakeDemage", 20, SendMessageOptions.DontRequireReceiver);
        }
        Instantiate(CrescentMoon, attackPoint.position + new Vector3(1 * direction, 0, 0), Quaternion.Euler(transform.eulerAngles + eulerAngles));
        isAttacking = false;
    }

    private void OnDrawGizmosSelected() {
        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, 1);
    }

    private void CastMagic(){
        whichMagic = rnd.Next(4,5);
        if(whichMagic == 1){
            Instantiate(Tornado, new Vector3(639, -95, 0), Tornado.transform.rotation);
            Instantiate(Tornado, new Vector3(655, -95, 0), Tornado.transform.rotation);
            Instantiate(Tornado, new Vector3(672, -95, 0), Tornado.transform.rotation);
            Instantiate(Tornado, new Vector3(692, -95, 0), Tornado.transform.rotation);
        }else if(whichMagic == 2){
            StartCoroutine(LightningStormCoroutine());
        }else if(whichMagic == 3){
            Instantiate(RedLotus, transform.position, transform.rotation);
        }else if(whichMagic == 4){
            useB52 = true;
        }

        isAttacking = false;
    }

    IEnumerator LightningStormCoroutine(){   
        for(int i = 0; i < 100; i++){
            int lightningPos = rnd.Next(-15, 12);
            Instantiate(LightningCloud, transform.position + new Vector3(lightningPos, 9, 0), LightningCloud.transform.rotation);
            Instantiate(LightningSpear, transform.position + new Vector3(lightningPos, 2, 0), LightningSpear.transform.rotation);
            yield return new WaitForSeconds(0.05f);
        }
    
    }

    private void GoToB52Pos(){
        if(onB52Pos){
            CastB52();
            return;
        }
        if(transform.position.y < -78){
            transform.Translate(Vector3.up * 2 * Time.deltaTime, Space.World);
            return;
        }
        animator.SetTrigger("CastB52");
        if(transform.position.x > 636){
            transform.Translate(Vector3.right * -1 * 12 * Time.deltaTime, Space.World);
            ChangeDirection("left");
            return;
        }
        onB52Pos = true;
    }

    private void CastB52(){
        if(diveB52){
            CastDiveB52();
            return;
        }
        if(transform.position.x < 690){
            transform.Translate(Vector3.right * 1 * 8 * Time.deltaTime, Space.World);
            ChangeDirection("right");
            if(transform.position.x > B52Rate){
                FindObjectOfType<AudioManager>().Play("CastFireBall");
                Instantiate(B52Bomb, transform.position, B52Bomb.transform.rotation);
                B52Rate += 4;
            }
            return;
        }
        diveB52 = true;
        var x = Player.transform.position.x - transform.position.x;
        var y = Player.transform.position.y - transform.position.y;
        ratio = x/y;
        ChargingFire.SetActive(true);
    }

    private void CastDiveB52(){
        if(transform.position.y > -85.8f){
            ChangeDirection("player");
            transform.Translate(Vector3.up * -6 * Time.deltaTime, Space.World);
            transform.Translate(Vector3.right * -6 * ratio * 1.5f * Time.deltaTime, Space.World);
            return;
        }
        useB52 = false;
        onB52Pos = false;
        diveB52 = false;
        ChargingFire.SetActive(false);
        animator.SetTrigger("StopB52");
    }

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
    }
}
