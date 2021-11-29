using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
 	private Transform playerTransform;

    void Start()
    {
    	playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate () {
        if ((playerTransform.position.x >= -6.5) && (playerTransform.position.x <= 84.5)){
        Vector3 temp = transform.position;
        temp.x = playerTransform.position.x;
        transform.position = temp;
        }
    }

}
