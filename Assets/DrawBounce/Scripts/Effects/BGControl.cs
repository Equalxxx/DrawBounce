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

	public int bgIndex;

	private float t;
	private Color startInnerColor;
	private Color endInnerColor;
	private Color startOuterColor;
	private Color endOuterColor;

	private bool isChanged;

#if UNITY_EDITOR
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			bgIndex++;
			if (bgIndex >= bgColorSetList.Count)
				bgIndex = 0;

			ChangeBGColor(bgIndex);
		}
	}
#endif

	public void ChangeBGColor(int bgIdx)
	{
		bgIndex = bgIdx;
		t = 0f;

		startInnerColor = sprInnerBG.color;
		endInnerColor = bgColorSetList[bgIndex].innerColor;
		startOuterColor = bgCamera.backgroundColor;
		endOuterColor = bgColorSetList[bgIndex].outerColor;

		StartCoroutine(ChangeBG());
	}

	IEnumerator ChangeBG()
	{
		if (isChanged)
			yield break;

		isChanged = true;

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

		isChanged = false;

		Debug.LogFormat("Change BG Color : {0}", bgIndex);
	}
}
