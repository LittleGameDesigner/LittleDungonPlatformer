using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public float moveSpeed = 20;
    public float demage = 8;
    private float ExistTime;
    // Start is called before the first frame update
    void Start()
    {
        ExistTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
        if(Time.time - ExistTime >= 1.5f){Destroy(gameObject);}
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player"){
            collision.SendMessage("TakeDemage", demage);
            FindObjectOfType<AudioManager>().Play("ArrowHitMetal");
            Destroy(gameObject);
        }
        if(collision.tag == "Terrian"){
            print("terrian");
            Destroy(gameObject);
        }   
    }
}

