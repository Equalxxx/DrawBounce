using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
	public float rotateAngle = 30f;
	public bool randomDir;

	private void OnEnable()
	{
		if(randomDir)
		{
			int rnd = Random.Range(0, 2);
			if (rnd == 0)
				rotateAngle = -rotateAngle;
		}
	}

	private void Update()
	{
		transform.Rotate(Vector3.forward * rotateAngle * Time.deltaTime);
	}
}
