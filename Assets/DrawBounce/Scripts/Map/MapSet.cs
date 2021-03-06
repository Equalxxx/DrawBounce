﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSet : MonoBehaviour, IPoolObject
{
	public float height;

	public SpriteRenderer areaRenderer;
	private DefaultBlock[] defaultBlocks;
	private BasicItem[] basicItems;

	private void OnValidate()
	{
		if (areaRenderer)
			height = areaRenderer.size.y;
	}

	public void InitMapSet()
	{
		if (defaultBlocks == null || defaultBlocks.Length == 0)
			defaultBlocks = GetComponentsInChildren<DefaultBlock>();

		if (basicItems == null || basicItems.Length == 0)
			basicItems = GetComponentsInChildren<BasicItem>();

		for (int i = 0; i < defaultBlocks.Length; i++)
		{
			defaultBlocks[i].InitBlock();
		}

		for(int i = 0; i < basicItems.Length; i++)
		{
			basicItems[i].InitItem();
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

		Debug.LogFormat("Spawn map set : {0}", name);
	}
}
