using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class ElectricBall : MonoBehaviour
{
    private GameObject Player;
    private float moveSpeed = 5;
    private float waitTime = 1;
    private float existTime = 8;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(waitTime > 0){
            waitTime -= Time.deltaTime;
            return;
        }
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        var ai = GetComponent<AIPath>();
        ai.destination = Player.transform.position;
        if(existTime <= 0){
            Destroy(gameObject);
        }
        existTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player"){
            collision.SendMessage("TakeDemage", 5);
            FindObjectOfType<AudioManager>().Play("CastFireBall");
            Destroy(gameObject);
        }
    }
}
