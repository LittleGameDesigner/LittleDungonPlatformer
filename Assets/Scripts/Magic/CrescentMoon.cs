using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrescentMoon : MonoBehaviour
{
    public float demage;
    public float moveSpeed;
    private float CD = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(transform.right * moveSpeed * Time.deltaTime, Space.World);        
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player"){
            if(CD >= 0.2f){
                collision.gameObject.SendMessage("TakeDemage", demage);
                FindObjectOfType<AudioManager>().Play("CastFireBall");
                CD = 0;
            }
            CD += Time.deltaTime;
        }
    }
}
