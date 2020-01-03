using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveBlock : DefaultBlock
{
	private SpriteRenderer sprRender;
	private BoxCollider2D boxCol2D;
	private Transform myTransform;
	private Transform playerTrans;

	public float offsetHeight = 0.5f;
	public float colorDuration = 1f;

	public bool isActivate;

	void Start()
    {
		sprRender = GetComponent<SpriteRenderer>();
		boxCol2D = GetComponent<BoxCollider2D>();
		myTransform = transform;
		playerTrans = GameManager.Instance.player.transform;

		Color sprColor = sprRender.color;
		sprColor.a = 0.1f;
		sprRender.color = sprColor;
	}

	private void Update()
	{
		if (isActivate)
			return;

		if (myTransform.position.y + offsetHeight < playerTrans.position.y)
		{
			isActivate = true;
			StartCoroutine(ShowBlock());
		}
	}                          

	IEnumerator ShowBlock()
	{
		boxCol2D.isTrigger = false;

		Color sprColor = sprRender.color;
		float alpha = sprColor.a;
		float t = 0f;

		while(t < 1f)
		{
			t += Time.deltaTime / colorDuration;
			sprColor.a = Mathf.Lerp(alpha, 1f, t);
			sprRender.color = sprColor;
			yield return null;
		}
	}
}
