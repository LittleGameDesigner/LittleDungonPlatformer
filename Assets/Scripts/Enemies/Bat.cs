using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Pathfinding;

public class Bat : MonoBehaviour
{
    //Material
    private GameObject Player;
    private Transform target;
    private Animator animator;
    Path path;
    Seeker seeker;
    Rigidbody2D rb;
    private Color originalColor;
    public LayerMask EnemyLayer;
    //Move
    public float speed = 400;
    public float nextWayPointDist = 3f;
    private int currentWayPoint = 0;
    private bool reachedEndOfPath;
    private float demage = 0;
    private bool waked;
    private bool dead;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        originalColor = PlayerRenderrer.material.color;
        
        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath(){
        if(seeker.IsDone()){
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p){
        if(!p.error){
            path = p;
            currentWayPoint = 0;
        }
    }

    void Update()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((XRange(Player) < 6) && (YRange(Player) < 10)){
            waked = true;
        }

        if(!waked || dead)return;
        animator.SetTrigger("Fly");

        if(path == null){
            return;
        }
        if(currentWayPoint >= path.vectorPath.Count){
            reachedEndOfPath = true;
            return;
        }else{
            reachedEndOfPath = false;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWayPoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWayPoint]);

        if(distance < nextWayPointDist){
            currentWayPoint++;
        }

        if((XRange(Player) < 3) && (YRange(Player) < 2)){
            rb.AddForce(direction * 8000 * Time.deltaTime);
        }

        if(force.x <= 0.01f){
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }else if(force.x >= -0.01f){
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }

    // private void OnTriggerEnter2D(Collider2D collision){
    //     if(collision.tag == "Player"){
    //         collision.SendMessage("TakeDemage", demage);
    //         if(rb.velocity.x <= 0.01f){
    //             rb.AddForce(new Vector2(-600, 100));
    //         }else if(rb.velocity.x >= -0.01f){
    //             rb.AddForce(new Vector2(600, 100));
    //         }
    //     }
    // }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player"){
            collision.gameObject.SendMessage("TakeDemage", demage);
            if(rb.velocity.x <= 0.01f){
                rb.AddForce(new Vector2(600, 100));
            }else if(rb.velocity.x >= -0.01f){
                rb.AddForce(new Vector2(-600, 100));
            }
        }
    }

    private void TakeDemage(float demage){
        dead = true;
        rb.AddForce(new Vector2(0, -2000));
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", Color.red);
        Invoke("DemageEffect", 0.2f);
        animator.SetTrigger("Die");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 100, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            enemy.SendMessage("GainEXP", 1, SendMessageOptions.DontRequireReceiver);
        }
        FindObjectOfType<AudioManager>().Play("SkeletonDie");
    }

    private void PushBack(int d){
        animator.SetTrigger("PushBack");
        rb.velocity = new Vector2(d, rb.velocity.y);
    }

    private void DemageEffect(){
        var PlayerRenderrer = gameObject.GetComponent<Renderer>();
        PlayerRenderrer.material.SetColor("_Color", originalColor);
    }

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
    }
}
