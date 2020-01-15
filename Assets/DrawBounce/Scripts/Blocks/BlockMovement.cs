using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{
	[Header("Move Position")]
	public bool isMove;
	public enum MovingBlockType { SideToSide, Repeat }
	public MovingBlockType moveType;
	public float moveWidth;
	public float moveSpeed;
	public float moveTime;

	[Header("Rotation")]
	public bool isRotate;
	public float rotateAngle;

	[Header("Scale")]
	public bool isScale;
	public float minScale;
	public float maxScale;
	public float scaleDuration;
	private float scaleTime;

	private Transform myTransform;
	private Vector3 originPosition;
	private Vector3 originScale;

	private void Awake()
	{
		myTransform = transform;
		originPosition = myTransform.position;
		originScale = myTransform.localScale;
	}

	private void Update()
	{
		if(isMove)
		{
			switch(moveType)
			{
				case MovingBlockType.SideToSide:
					break;
				case MovingBlockType.Repeat:
					moveTime += moveSpeed * Time.deltaTime;
					//myTransform.Translate(myTransform.right * moveSpeed * Time.deltaTime);
					break;
			}
		}

		if(isRotate)
		{
			transform.Rotate(Vector3.forward * rotateAngle * Time.deltaTime);
		}

		if(isScale)
		{
			scaleTime += Time.deltaTime / scaleDuration;
		}
	}
}
