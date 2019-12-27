using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform myTransform;
    public Transform targetTrans;
    public float followSpeed = 5f;

    private void Awake()
    {
        myTransform = transform;
    }

    private void Update()
    {
        //Vector3 pos = Vector3.Lerp(myTransform.position, targetTrans.position, Time.deltaTime * followSpeed);
        Vector3 pos = targetTrans.position;
        pos.x = 0f;
        if (pos.y < 0f)
            pos.y = 0f;
        pos.z = myTransform.position.z;
        myTransform.position = pos;
    }
}
