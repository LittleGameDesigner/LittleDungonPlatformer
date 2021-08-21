using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject Door;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player"){
            Door door = Door.GetComponent<Door>();
            FindObjectOfType<AudioManager>().Play("Ding");
            door.hasKey = true;
            Destroy(gameObject);
        }
    }
}
