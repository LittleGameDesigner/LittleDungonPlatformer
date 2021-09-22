using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tornado : MonoBehaviour
{
    public float demage;
    public float moveSpeed;
    public string tag = "Enemy";
    private float CD = 0.2f;
    private float DestroyTime = 6;
    public LayerMask EnemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<AudioManager>().Play("Tornado");
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 8, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            int d = CalculatePushDirection(enemy);
            enemy.SendMessage("PushBack", d, SendMessageOptions.DontRequireReceiver);
        }
        hitEnemies = Physics2D.OverlapCircleAll(transform.position, 7, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            if(CD >= 0.2f){
                enemy.SendMessage("TakeDemage", demage);
                CD = 0;
            }
        }
        DestroyTime -= Time.deltaTime;
        if(DestroyTime <= 0){
            FindObjectOfType<AudioManager>().Stop("Tornado");
            Destroy(gameObject);
        }
        CD += Time.deltaTime;
    }

    private int CalculatePushDirection(Collider2D enemy){
        if(enemy.transform.position.x > transform.position.x){
            return -8;
        }else{
            return 8;
        }
    }
}
