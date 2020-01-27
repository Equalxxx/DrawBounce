using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObject : MonoBehaviour
{
	public enum MovingBlockType { Repeat, SideToSide }
	public MovingBlockType movingType;

	public float duration = 3f;

	private Transform myTransform;
	private Vector3 originPosition;
	public Vector3 targetPosition;

	void Awake()
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
		Vector3 startPos = originPosition;
		Vector3 targetPos = targetPosition;

		float t = 0f;

		while (true)
		{
			t += Time.deltaTime / duration;

			myTransform.localPosition = Vector3.Lerp(startPos, targetPos, t);

			if (t >= 1f)
			{
				t = 0f;
				switch(movingType)
				{
					case MovingBlockType.SideToSide:
						Vector3 temp = startPos;
						startPos = targetPos;
						targetPos = temp;
						break;
					case MovingBlockType.Repeat:
						myTransform.localPosition = startPos;
						break;
				}

				myTransform.localPosition = startPos;
			}

			yield return null;
		}
	}
}
