using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargingFire : MonoBehaviour
{
    private Transform player;
    private float PlayBGM = 0;
    public float demageDelay;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0].transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayBGM >= 0.3f){
            print("play");
            FindObjectOfType<AudioManager>().Play("CastFireBall");
            PlayBGM = 0;
        }
        PlayBGM += Time.deltaTime;
        demageDelay -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(demageDelay > 0)return;
        if(collision.tag == "Player"){
            collision.SendMessage("TakeDemage", 50);
            if(player.position.x > transform.position.x){
                collision.SendMessage("PushBack", 40);
            }else{
                collision.SendMessage("PushBack", -40); 
                }
        }
    }
}
