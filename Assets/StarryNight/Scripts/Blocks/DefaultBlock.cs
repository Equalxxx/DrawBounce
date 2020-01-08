using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBlock : MonoBehaviour, IPoolObject
{

	void OnEnable()
	{
		GameManager.GameOverAction += DisableBlock;
	}

	void OnDisable()
	{
		GameManager.GameOverAction -= DisableBlock;
	}

	public virtual void OnSpawnObject() { }

	void DisableBlock()
	{
		gameObject.SetActive(false);
	}

	public virtual void ShowBlock(bool show)
	{
		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);
	}
}
