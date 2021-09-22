using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoSpell : MonoBehaviour
{
    public GameObject TornadoLock;
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
            var playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
            playerController.GotTornado = true;
            //TornadoLock.SetActive(false);
            FindObjectOfType<AudioManager>().Play("Ding");
            Destroy(gameObject);
        }
    }
}
