using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportGate : MonoBehaviour
{
    private GameObject PlayerData;
    // Start is called before the first frame update
    void Start()
    {
        PlayerData = GameObject.Find("StaticPlayerData");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.tag == "Player"){
            SceneManager.LoadScene("Chapter2");
            GameObject.Find("PlayerSword").GetComponent<PlayerSword>().transform.position = new Vector3(-30, 0, 0);
            DontDestroyOnLoad(PlayerData);
        }
    }
}
