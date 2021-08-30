using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerSword;
    public GameObject PlayerBow;
    public GameObject Menu;

    void Start()
    {
        
    }

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

        if(Input.GetKeyDown(KeyCode.Escape)){
            Menu.SetActive(true);
        }
    }

}
