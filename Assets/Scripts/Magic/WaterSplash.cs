using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSplash : MonoBehaviour
{
    public string target = "Enemy"; 
    public float demage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == target){
            collision.SendMessage("PushUp", 40, SendMessageOptions.DontRequireReceiver);
            collision.SendMessage("TakeDemage", demage);
        }
    }
}
