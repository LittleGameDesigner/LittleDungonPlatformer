using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B52_Moon : MonoBehaviour
{
    public float demage = 50;
    public LayerMask EnemyLayer;
    public GameObject explosionEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Player"){
            Instantiate(explosionEffect, transform.position, transform.rotation);
            FindObjectOfType<AudioManager>().Play("FireBallExplosion");
            Destroy(gameObject);
        }else if(collision.tag == "Terrian"){
            Instantiate(explosionEffect, transform.position, transform.rotation);
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 3, EnemyLayer);
            foreach (Collider2D enemy in hitEnemies){
                enemy.SendMessage("TakeDemage", demage, SendMessageOptions.DontRequireReceiver);
            }
            FindObjectOfType<AudioManager>().Play("FireBallExplosion");
            Destroy(gameObject);
        }
    }
}
