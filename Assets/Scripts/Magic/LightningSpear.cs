using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningSpear : MonoBehaviour
{
    public float demage;
    public string tag = "Enemy";
    private float destroyTime = 0.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(destroyTime <= 0){
            Destroy(gameObject);
        }
        destroyTime -= Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == tag){
            collision.SendMessage("TakeDemage", demage);
        }
    }
}
