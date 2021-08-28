using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player"){
            collision.SendMessage("ChangeBow");
            FindObjectOfType<AudioManager>().Play("DrawBow");
            Destroy(gameObject);
        }
    }

}
