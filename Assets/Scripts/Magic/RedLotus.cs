using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedLotus : MonoBehaviour
{
    public float demage;
    public LayerMask EnemyLayer;
    private bool demaged;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(demaged)return;
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 13, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            int d = CalculatePushDirection(enemy);
            enemy.SendMessage("PushBack", d, SendMessageOptions.DontRequireReceiver);
        }
        hitEnemies = Physics2D.OverlapCircleAll(transform.position, 13, EnemyLayer);
        foreach (Collider2D enemy in hitEnemies){
            demaged = true;
            enemy.SendMessage("TakeDemage", demage);
        }
    }

    private int CalculatePushDirection(Collider2D enemy){
        if(enemy.transform.position.x > transform.position.x){
            return 120;
        }else{
            return -120;
        }
    }
}
