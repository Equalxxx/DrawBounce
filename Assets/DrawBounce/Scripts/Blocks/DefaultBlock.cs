using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultBlock : MonoBehaviour, IPoolObject
{
	public virtual void OnSpawnObject() { }

	public virtual void ShowBlock(bool show)
	{
		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);
	}
}
