using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraPlayer : MonoBehaviour
{
    public Transform target;
    public float speed = 1f;
    private Vector3 targetPos;
    private Vector3 thisPos;
    private float angle;
    public CinemachineVirtualCamera Camera;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Camera = GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(target == null){
            GameObject[] possibleTargets;
            possibleTargets = GameObject.FindGameObjectsWithTag("Player");
            if(possibleTargets.Length > 0){
                target = possibleTargets[0].transform;
            }
        }else{
            Camera.Follow = target;
            // Vector3 targetPos = target.transform.position;
            // Vector3 targetFlattened = new Vector3(targetPos.x, targetPos.y, 0);
            // transform.LookAt(targetFlattened);
        }
    }
}
