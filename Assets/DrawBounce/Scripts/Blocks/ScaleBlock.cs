using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleBlock : DefaultBlock
{
	public float minScale = 2f;
	public float maxScale = 3f;

	public float minDuration = 2.5f;
	public float maxDuration = 5f;

	public bool isRandomRotate;

	private DOTweenAnimation scaleAnimation;
	private Transform myTransform;

	private void Awake()
	{
		myTransform = transform;
		scaleAnimation = GetComponent<DOTweenAnimation>();

		InitBlock();
	}

	public override void InitBlock()
	{
		if (!myTransform)
			return;

		myTransform.localScale = Vector3.one;

		if (isRandomRotate)
			myTransform.localEulerAngles = Vector3.forward * Random.Range(0f, 360f);
		
		scaleAnimation.endValueFloat = Random.Range(minScale, maxScale);
		scaleAnimation.duration = Random.Range(minDuration, maxDuration);

		scaleAnimation.DORestart();
	}
}
