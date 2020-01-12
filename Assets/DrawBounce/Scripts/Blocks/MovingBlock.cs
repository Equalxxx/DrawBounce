using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBlock : DefaultBlock
{
	public enum MovingBlockType { SideToSide, Repeat }
	public MovingBlockType movingType;
	
	public float moveFactor = 3f;

	private Transform myTransform;
	private Vector3 originPosition;

	private void Awake()
	{
		myTransform = transform;
		originPosition = myTransform.localPosition;
	}

	private void OnEnable()
	{
		StartCoroutine(Moving());
	}

	private void OnDisable()
	{
		StopCoroutine(Moving());
	}

	IEnumerator Moving()
	{
		switch (movingType)
		{
			case MovingBlockType.SideToSide:
				Vector3 dir = Vector3.right;
				bool swap = true;
				while(true)
				{
					if (swap)
					{
						if (myTransform.localPosition.x > Mathf.Abs(originPosition.x))
						{
							dir.x = -1f;
							swap = false;
						}
					}
					else
					{
						if (myTransform.localPosition.x < -Mathf.Abs(originPosition.x))
						{
							dir.x = 1f;
							swap = true;
						}
					}

					myTransform.Translate(dir * moveFactor * Time.deltaTime);
					yield return null;
				}
			case MovingBlockType.Repeat:
				Vector3 pos = myTransform.localPosition;
				float t = 0f;

				while(true)
				{
					t += Time.deltaTime / moveFactor;
					
					if(originPosition.x >= 0f)
						pos.x = originPosition.x - Mathf.Repeat(t, Mathf.Abs(originPosition.x * 2f));
					else
						pos.x = originPosition.x + Mathf.Repeat(t, Mathf.Abs(originPosition.x * 2f));

					myTransform.localPosition = pos;

					yield return null;
				}
		}
	}
}
