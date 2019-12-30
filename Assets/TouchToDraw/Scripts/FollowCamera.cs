using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform myTransform;
    public Transform targetTrans;
    public float followSpeed = 5f;
    public float bottomHeight = 4f;

    private void Awake()
    {
        myTransform = transform;
        targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        //Vector3 pos = Vector3.Lerp(myTransform.position, targetTrans.position, Time.deltaTime * followSpeed);
        Vector3 pos = targetTrans.position;
        pos.x = 0f;
        if (pos.y < bottomHeight)
            pos.y = bottomHeight;

        pos.z = myTransform.position.z;
        myTransform.position = pos;
    }
}
