using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AddScoreEffect : MonoBehaviour, IPoolObject
{
	public float duration = 1f;
	private DOTweenAnimation tweenAnim;

	private void Awake()
	{
		tweenAnim = GetComponentInChildren<DOTweenAnimation>();
		tweenAnim.duration = duration;
	}

	public void OnSpawnObject()
	{
		tweenAnim.DORestart();
		StartCoroutine(AutoDisable());
	}

	IEnumerator AutoDisable()
	{
		yield return new WaitForSeconds(duration);

		gameObject.SetActive(false);
	}
}
