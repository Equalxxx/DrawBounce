using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleObject : MonoBehaviour
{
	public bool scaleX = true;
	public bool scaleY = true;

	public float minScale = 1f;
	public float maxScale = 3f;

	public float duration = 1f;
	public float delay;

	private Transform myTransform;
	private Vector2 originScale;

	private void Awake()
	{
		myTransform = transform;
		originScale = myTransform.localScale;
	}

	private void OnEnable()
	{
		StartCoroutine(ScaleAnim());
	}

	private void OnDisable()
	{
		StopCoroutine(ScaleAnim());
	}

	IEnumerator ScaleAnim()
    {
		myTransform.localScale = originScale;

		Vector2 scale = myTransform.localScale;
		float startScale = 0f;
		float endScale = 0f;
		float t = 0f;
		bool swap = false;

		yield return new WaitForSeconds(delay);

		while (true)
		{
			t += Time.deltaTime / duration;

			if (t >= 1f)
			{
				t = 0f;
				swap = !swap;
				yield return new WaitForSeconds(delay);
			}

			if(!swap)
			{
				startScale = minScale;
				endScale = maxScale;
			}
			else
			{
				startScale = maxScale;
				endScale = minScale;
			}


			if (scaleX)
				scale.x = Mathf.Lerp(startScale, endScale, t);

			if (scaleY)
				scale.y = Mathf.Lerp(startScale, endScale, t);

			myTransform.localScale = scale;

			yield return null;
		}
    }
}
