using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    //Materials
    public int whichPotion;
    private GameObject Player;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player"){
            switch(whichPotion){
                case 0:
                    collision.SendMessage("DrinkRedPotion");
                    FindObjectOfType<AudioManager>().Play("DrinkPotion");
                    Destroy(gameObject);
                    break;
                case 1:
                    collision.SendMessage("DrinkYellowPotion");
                    FindObjectOfType<AudioManager>().Play("DrinkPotion");
                    Destroy(gameObject);
                    break;
                default:
                    break;
            }
            
        }
    }
}
