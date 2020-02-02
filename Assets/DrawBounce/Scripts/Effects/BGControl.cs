using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGControl : MonoBehaviour
{
	[System.Serializable]
	public class BGColorSet
	{
		public Color innerColor;
		public Color outerColor;
	}

	public SpriteRenderer sprInnerBG;
	public Camera bgCamera;
	public List<BGColorSet> bgColorSetList;

	public float duration = 1f;

	public float delay = 3f;
	public int colorIdx;
	public int testIdx;

#if UNITY_EDITOR
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			ChangeBGColor();
		}
	}
#endif

	public void ChangeBGColor(bool first = false)
	{
		if (first)
		{
			colorIdx = 0;
		}
		else
		{
			colorIdx++;
			if (colorIdx >= bgColorSetList.Count)
				colorIdx = 0;
		}

		StartCoroutine(ChangeBG());
	}

	IEnumerator ChangeBG()
	{
		Color startInnerColor = sprInnerBG.color;
		Color endInnerColor = bgColorSetList[colorIdx].innerColor;
		Color startOuterColor = bgCamera.backgroundColor;
		Color endOuterColor = bgColorSetList[colorIdx].outerColor;

		float t = 0f;

		while (true)
		{
			t += Time.deltaTime / duration;
			sprInnerBG.color = Color.Lerp(startInnerColor, endInnerColor, t);
			bgCamera.backgroundColor = Color.Lerp(startOuterColor, endOuterColor, t);

			if (t >= 1f)
			{
				break;
			}

			yield return null;
		}

		Debug.LogFormat("Change BG Color : {0}", colorIdx);
	}
}
