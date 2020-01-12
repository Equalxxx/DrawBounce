using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerCollider : MonoBehaviour, IPoolObject
{
	private void OnEnable()
	{
		GameManager.PauseAction += GamePause;
	}

	private void OnDisable()
	{
		GameManager.PauseAction -= GamePause;
	}

	void GamePause(bool pause)
	{
		if (pause)
			Show(false);
	}
	void Show(bool show)
	{
		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);
	}

	public void OnSpawnObject()
	{

	}
}
