using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleportGate : MonoBehaviour
{
    private GameObject PlayerData;
    public GameObject LoadingPage;
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
            LoadingPage.SetActive(true);
            SceneManager.LoadScene("Chapter2");
            var playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
            playerController.transform.position = new Vector3(-2.6f, -0.8f, 0);
            SaveManager.Save(playerController, "Chapter2");
        }
    }
}
