using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class AddCoinEffect : MonoBehaviour, IPoolObject
{
	public float duration = 1f;
	//private DOTweenAnimation tweenAnim;
	private TextMeshPro coinText;

	private void Awake()
	{
		coinText = GetComponentInChildren<TextMeshPro>();
		//tweenAnim = GetComponentInChildren<DOTweenAnimation>();
		//tweenAnim.duration = duration;
	}

	public void OnSpawnObject()
	{
		//tweenAnim.DORestart();
		StartCoroutine(AutoDisable());
	}

	public void RefreshEffect(int coinValue)
	{
		coinText.text = string.Format("+{0}", coinValue);
	}

	IEnumerator AutoDisable()
	{
		yield return new WaitForSeconds(duration);

		gameObject.SetActive(false);
	}
}
