using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PosMap
{
	public int index;
	public Vector2 position;
	public GameObject blockObject;

	public void Show(bool show)
	{
		if (blockObject)
		{
			if (blockObject.activeSelf != show)
			{
				blockObject.SetActive(show);
			}
		}
	}
}

public class TestPosMap : MonoBehaviour
{
	public Transform target;
	public Vector3 targetPos;
	public float height;
	public List<PosMap> posMapList;

	public GameObject testPrefab;

	private void Start()
	{
		for (int i = 0; i < 3; i++)
		{
			PosMap posMap = new PosMap();
			posMap.index = i;
			posMap.position = new Vector2(0f, i * height);
			posMap.blockObject = Instantiate(testPrefab, posMap.position, Quaternion.identity);
			posMapList.Add(posMap);
		}
	}

	private void Update()
	{
		targetPos = target.position;

		if (targetPos.y <= 0f)
			return;

		if (posMapList.Exists(x => x.position.y - height <= targetPos.y && x.position.y >= targetPos.y))
		{
			for (int i = 0; i < posMapList.Count; i++)
			{
				if (posMapList[i].position.y - height <= targetPos.y && posMapList[i].position.y + height >= targetPos.y)
				{
					posMapList[i].Show(true);
				}
				else
				{
					posMapList[i].Show(false);
				}
			}
		}
		else
		{
			PosMap posMap = new PosMap();
			posMap.index = posMapList.Count;
			posMap.position = new Vector2(0f, posMapList.Count * height);
			posMap.blockObject = Instantiate(testPrefab, posMap.position, Quaternion.identity);
			posMapList.Add(posMap);
		}
	}
}