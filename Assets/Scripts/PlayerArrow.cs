using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerArrow : MonoBehaviour
{
    public float moveSpeed = 10;
    public float demage = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Enemy"){
            collision.SendMessage("TakeDemage", demage);
            FindObjectOfType<AudioManager>().Play("ArrowHitMetal");
            Destroy(gameObject);
        }
        
    }
}
