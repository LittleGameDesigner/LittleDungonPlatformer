using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerRageSpell : MonoBehaviour
{
    public GameObject PlayerBow;
    public GameObject RageLock;
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
            PlayerBow pb = PlayerBow.GetComponent<PlayerBow>();
            pb.GotBerserkerRage = true;
            PlayerData.GetComponent<StaticPlayerData>().GotBerserkerRage = true;
            RageLock.SetActive(false);
            FindObjectOfType<AudioManager>().Play("BerserkerRage");
            Destroy(gameObject);
        }
    }
}
