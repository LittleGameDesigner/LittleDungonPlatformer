using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FirstSword : MonoBehaviour
{
    private GameObject Player;
    private bool gotHint;
    public GameObject Hint;
    public Text HintText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Player = GameObject.FindGameObjectsWithTag("Player")[0];
        if((XRange(Player) < 6) && (YRange(Player) < 4) && !gotHint){
            Hint.SetActive(true);
            HintText.text = "You can pick up a weapon by touching it!";
            Time.timeScale = 0;

            if(Input.anyKeyDown){
                Time.timeScale = 1;
                gotHint = true;
                Hint.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player"){
            collision.SendMessage("ChangeSword");
            FindObjectOfType<AudioManager>().Play("DrawSword");
            Destroy(gameObject);
        }
    }

    private float XRange(GameObject player){
        return Math.Abs(Player.transform.position.x - transform.position.x);
    }

    private float YRange(GameObject player){
        return Math.Abs(Player.transform.position.y - transform.position.y);
    }
}
