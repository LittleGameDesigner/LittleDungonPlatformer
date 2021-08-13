using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBoss : MonoBehaviour
{
    private float TurnOverTime = 0;
    private int direction = 1;
    public float MoveSpeed = 2;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        Move();
        if(TurnOverTime > 3){
            direction *= -1;
            TurnOverTime = 0;
            animator.SetTrigger("Attack1");
        }
        TurnOverTime += Time.fixedDeltaTime;
    }

    private void Move(){
        
        if(TurnOverTime > 3){
            Vector3 facingDirection = transform.localScale;
            facingDirection.x *= -1;
            transform.localScale = facingDirection;
        }
        transform.Translate(Vector3.right * MoveSpeed * direction * Time.fixedDeltaTime, Space.World);

    }
}
