using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGControl : MonoBehaviour
{
	private SpriteRenderer sprRenderer;
	public Color[] bgColors;
	public float duration = 1f;

	public float delay = 3f;

	IEnumerator Start()
	{
		sprRenderer = GetComponent<SpriteRenderer>();

		float t = 0f;
		int rndIdx = Random.Range(0, bgColors.Length);

		Color startColor = sprRenderer.color;
		Color endColor = bgColors[rndIdx];

		while (true)
		{
			t += Time.deltaTime / duration;
			sprRenderer.color = Color.Lerp(startColor, endColor, t);

			if (t >= 1f)
			{
				t = 0f;
				rndIdx = Random.Range(0, bgColors.Length);
				startColor = sprRenderer.color;
				endColor = bgColors[rndIdx];

				yield return new WaitForSeconds(delay);
			}

			yield return null;
		}
	}
}
