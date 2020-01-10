using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Transform myTransform;
    private Transform targetTrans;
	private Vector3 originPosition;
	private Vector3 targetPos;

	public float lastHeight;
	public float offset = 5f;

	public float smoothFactor = 10f;
	public float limitWidth = 1f;

	private bool isFollow;

    private void Awake()
    {
        myTransform = transform;
		originPosition = myTransform.position;

        targetTrans = GameObject.FindGameObjectWithTag("Player").transform;
    }

	private void OnEnable()
	{
		GameManager.GameInitAction += Init;
		GameManager.GamePlayAction += StartFollow;
	}

	private void OnDisable()
	{
		GameManager.GameInitAction -= Init;
		GameManager.GamePlayAction -= StartFollow;
	}

	void Init()
	{
		lastHeight = 0f;
		targetPos = Vector3.zero;
		isFollow = false;
		myTransform.position = originPosition;
	}

	void StartFollow()
	{
		if (!isFollow)
			isFollow = true;
	}

    private void FixedUpdate()
    {
		if (!isFollow)
			return;

		if (lastHeight < targetTrans.position.y)
		{
			lastHeight = targetTrans.position.y;
		}

		targetPos = Vector3.Lerp(myTransform.position, targetTrans.position, Time.fixedDeltaTime * smoothFactor);

		if (targetPos.x > limitWidth)
			targetPos.x = limitWidth;
		if (targetPos.x < -limitWidth)
			targetPos.x = -limitWidth;

		targetPos.z = 0f;

		if (lastHeight - offset < targetTrans.position.y && targetTrans.position.y > 0f)
		{

		}
		else
		{
			targetPos.y = myTransform.position.y;
		}

		myTransform.position = targetPos;
	}
}
