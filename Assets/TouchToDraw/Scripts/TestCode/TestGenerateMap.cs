using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoyconFramework;

[System.Serializable]
public class MapPosition
{
	public int index;
	public Vector2 position;
	public string blockTag;
	public GameObject blockPrefab;
	public GameObject blockObject;
	public float dist;
}

public class TestGenerateMap : Singleton<TestGenerateMap>
{
	public Transform playerTrans;
	public Vector2 playerPosition;
	public List<MapPosition> mapPosList;
	public MapPosition curMapPos;
	public float spacing = 10f;

	public GameObject[] blockPrefab;

	public float cameraSize = 5f;

	private void OnValidate()
	{
		mapPosList.Clear();

		for (int i = 0; i < 3; i++)
		{
			MapPosition mapPos = new MapPosition();
			mapPos.index = i;
			mapPos.position = new Vector2(0, i * spacing);
			int rnd = Random.Range(0, blockPrefab.Length);
			mapPos.blockTag = blockPrefab[rnd].name;
			mapPos.blockPrefab = blockPrefab[rnd];
			mapPosList.Add(mapPos);
		}
	}

	private void Update()
	{
		playerPosition = Camera.main.transform.position;

		CheckMapPos();

		for (int i = 0; i < mapPosList.Count; i++)
		{
			Vector2 mapPos = mapPosList[i].position;
			Vector2 camPos = Camera.main.transform.position;
			mapPosList[i].dist = camPos.y - mapPos.y;

			if (mapPosList[i].dist < cameraSize * 2f && mapPosList[i].dist > -cameraSize * 2f)
			{
				if (!mapPosList[i].blockObject)
				{
					mapPosList[i].blockObject = Instantiate(mapPosList[i].blockPrefab, mapPosList[i].position, mapPosList[i].blockPrefab.transform.rotation);
				}
			}
			else
			{
				if(mapPosList[i].blockObject)
					Destroy(mapPosList[i].blockObject);
			}
		}
	}

	void CheckMapPos()
	{
		for (int i = 0; i < mapPosList.Count; i++)
		{
			Vector2 mapPos = mapPosList[i].position;
			Vector2 camPos = Camera.main.transform.position;
			mapPosList[i].dist = camPos.y - mapPos.y;

			if (mapPosList[i].dist < cameraSize/2f && mapPosList[i].dist > -(cameraSize/2f))
			{
				return;
			}
		}

		MapPosition mapPosInfo = new MapPosition();
		mapPosInfo.index = mapPosList.Count;
		mapPosInfo.position = new Vector2(0, mapPosList.Count * spacing);
		int rnd = Random.Range(0, blockPrefab.Length);
		mapPosInfo.blockTag = blockPrefab[rnd].name;
		mapPosInfo.blockPrefab = blockPrefab[rnd];

		mapPosList.Add(mapPosInfo);
	}
}
