using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Traps : MonoBehaviour
{
    private float demageCD = .2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision) {
        if(collision.tag == "Player" || collision.tag == "Enemy"){
            if(demageCD > .2f){
                collision.SendMessage("TakeDemage", 10);
                demageCD = 0;
            }
            demageCD += Time.deltaTime;
        }
    }
}
