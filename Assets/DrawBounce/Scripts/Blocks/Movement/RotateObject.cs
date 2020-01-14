using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateObject : MonoBehaviour
{
	public float rotateAngle = 30f;

	private void Update()
	{
		transform.Rotate(Vector3.forward * rotateAngle * Time.deltaTime);
	}
}
