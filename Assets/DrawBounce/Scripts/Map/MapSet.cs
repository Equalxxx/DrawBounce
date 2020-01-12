using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSet : MonoBehaviour, IPoolObject
{
	public float height;

	public SpriteRenderer areaRenderer;
	private DefaultBlock[] defaultBlocks;

	private void OnValidate()
	{
		InitMapSet();
	}

	public void InitMapSet()
	{
		if(areaRenderer)
			height = areaRenderer.size.y;
		if (defaultBlocks == null)
			defaultBlocks = GetComponentsInChildren<DefaultBlock>();

		for (int i = 0; i < defaultBlocks.Length; i++)
		{
			defaultBlocks[i].InitBlock();
		}
	}

	public void Show(bool show)
	{
		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);
	}

	public void OnSpawnObject()
	{
		Show(true);
		InitMapSet();
	}
}
