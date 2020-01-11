using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSet : MonoBehaviour, IPoolObject
{
	public float height;

	public SpriteRenderer areaRenderer;
	public DefaultBlock[] defaultBlocks;

	private void OnValidate()
	{
		Init();
	}

	private void Start()
	{
		Init();
	}

	void Init()
	{
		if(areaRenderer)
			height = areaRenderer.size.y;
		if (defaultBlocks == null)
			defaultBlocks = GetComponentsInChildren<DefaultBlock>();
	}

	public void Show(bool show)
	{
		if (gameObject.activeSelf != show)
			gameObject.SetActive(show);

		//for (int i = 0; i < defaultBlocks.Length; i++)
		//{
		//	if (defaultBlocks[i].gameObject.activeSelf != show)
		//		defaultBlocks[i].gameObject.SetActive(show);
		//}
	}

	public void OnSpawnObject()
	{
		Show(true);
	}
}
