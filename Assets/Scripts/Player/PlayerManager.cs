using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerSword;
    public GameObject PlayerBow;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(GameObject.Find("Player") != null){
            PlayerSword.transform.position = Player.transform.position;
            PlayerBow.transform.position = Player.transform.position;
        }
        else if(GameObject.Find("PlayerSword") != null){
            Player.transform.position = PlayerSword.transform.position;
            PlayerBow.transform.position = PlayerSword.transform.position;
        }else if(GameObject.Find("PlayerBow") != null){
            Player.transform.position = PlayerBow.transform.position;
            PlayerSword.transform.position = PlayerBow.transform.position;
        }
    }
}
