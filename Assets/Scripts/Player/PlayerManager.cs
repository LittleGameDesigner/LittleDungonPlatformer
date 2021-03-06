using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public GameObject Player;
    public GameObject PlayerSword;
    public GameObject PlayerBow;
    public GameObject PlayerStat;

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
            if(PlayerStat.activeSelf){
                PlayerStat.SetActive(false);
            }else{
                PlayerStat.SetActive(true);
            }
        }
    }

    public void ChangeWeapon(string weapon){
        if(weapon == "PlayerSword"){
            PlayerSword.SetActive(true);
            Player.SetActive(false);
            PlayerBow.SetActive(false);
        }
    }
}
