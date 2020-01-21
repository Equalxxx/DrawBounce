using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ShowMessageUI : MonoBehaviour
{
	public DOTweenAnimation anim;
	public bool isPlaying;
	public float closeDelay = 2f;
	public LocalizeTextMeshPro localizeText;

	public void Show(int stringIdx)
	{
		if (isPlaying)
			return;

		localizeText.stringIndex = stringIdx;
		localizeText.ShowLocalize();

		StartCoroutine(StartAnim());
	}

	IEnumerator StartAnim()
	{
		isPlaying = true;

		anim.DOPlayForward();
		yield return new WaitForSeconds(closeDelay);

		anim.DOPlayBackwards();
		yield return new WaitForSeconds(0.5f);

		isPlaying = false;
	}
}
