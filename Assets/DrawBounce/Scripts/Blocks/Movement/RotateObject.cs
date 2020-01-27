using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
	public float rotateAngle = 30f;
	public bool randomDir;
	public bool isRandomStartRotate;

	private Transform myTransform;

	private void OnEnable()
	{
		if (myTransform == null)
			myTransform = transform;

		if (randomDir)
		{
			int rnd = Random.Range(0, 2);
			if (rnd == 0)
				rotateAngle = -rotateAngle;
		}

		if(isRandomStartRotate)
		{
			myTransform.localEulerAngles = Vector3.forward * Random.Range(0f, 360f);
		}
	}

	private void Update()
	{
		myTransform.Rotate(Vector3.forward * rotateAngle * Time.deltaTime);
	}
}
