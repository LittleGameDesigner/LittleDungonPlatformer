using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    public float moveSpeed = 10;
    public float demage = 50;
    private float ExistTime;
    public GameObject Explosion;
    public LayerMask EnemyLayer;

    void Start()
    {
        ExistTime = Time.time;
    }

    void Update()
    {
        transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
        if(Time.time - ExistTime >= 3){Destroy(gameObject);}
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Enemy"){
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 3, EnemyLayer);
            foreach (Collider2D enemy in hitEnemies){
                enemy.SendMessage("TakeDemage", demage, SendMessageOptions.DontRequireReceiver);
            }
            FindObjectOfType<AudioManager>().Play("FireBallExplosion");
            Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(gameObject);
        }
        
    }
}
