using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallSpell : MonoBehaviour
{
    public GameObject PlayerSword;
    public GameObject FireBallLock;
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
            PlayerSword ps = PlayerSword.GetComponent<PlayerSword>();
            ps.GotFireBall = true;
            PlayerData.GetComponent<StaticPlayerData>().GotFireBall = true;
            FireBallLock.SetActive(false);
            FindObjectOfType<AudioManager>().Play("CastFireBall");
            Destroy(gameObject);
        }
    }
}
