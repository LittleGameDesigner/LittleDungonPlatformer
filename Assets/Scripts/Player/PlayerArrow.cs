using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    public float moveSpeed = 20;
    public float demage;

    void Start()
    {
        
    }

    void Update()
    {
        transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.name.Contains("SkeletonShield")){
            FindObjectOfType<AudioManager>().Play("Block");
            Destroy(gameObject);
        }
        if(collision.tag == "Enemy"){
            collision.SendMessage("TakeDemage", demage);
            FindObjectOfType<AudioManager>().Play("ArrowHitMetal");
            Destroy(gameObject);
        }  
    }
}
