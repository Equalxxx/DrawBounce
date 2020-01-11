using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFollowCam : MonoBehaviour
{
	public Transform targetTrans;
	public float lastHeight;
	public float offset;
	public float smoothFactor = 10f;
	private Transform myTransform;

	private void Awake()
	{
		myTransform = transform;
	}

	void Update()
    {
		if (lastHeight < targetTrans.position.y)
		{
			lastHeight = targetTrans.position.y;
		}

		if (lastHeight - offset < targetTrans.position.y)
		{
			myTransform.position = Vector3.Lerp(myTransform.position, targetTrans.position, Time.deltaTime * smoothFactor);
		}
    }
}
