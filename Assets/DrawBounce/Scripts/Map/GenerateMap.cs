using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

[System.Serializable]
public class MapPos
{
	public int index;
	public MapSet mapSet;

	public void Remove()
	{
		mapSet.Show(false);
		mapSet = null;
	}
}

public class GenerateMap : MonoBehaviour
{
	public float lastMapHeight;
	public float lastMapPosition = 10f;
	public float firstMapPosition;

	public int mapCount;

	public float mapLevRandomPercent = 70f;

	public List<MapPos> mapPosList;

	private Player player;

	public List<string> mapTagList_1;
	public List<string> mapTagList_2;

	private void Awake()
	{
		PrepareAssets();
	}

	private void OnEnable()
	{
		GameManager.GameInitAction += InitMap;
	}

	private void OnDisable()
	{
		GameManager.GameInitAction -= InitMap;
	}

	void InitMap()
	{
		player = GameManager.Instance.player;
		
		lastMapHeight = 0f;
		lastMapPosition = 10f;
		mapCount = 0;

		for (int i = 0; i < mapPosList.Count; i++)
		{
			mapPosList[i].Remove();
		}

		mapPosList.Clear();

		for (int i = 0; i < 4; i++)
		{
			AddMapPos();
		}
	}

	void PrepareAssets()
	{
		for (int i = 0; i < mapTagList_1.Count; i++)
		{
			PoolManager.Instance.PrepareAssets(mapTagList_1[i]);
		}
		for (int i = 0; i < mapTagList_2.Count; i++)
		{
			PoolManager.Instance.PrepareAssets(mapTagList_2[i]);
		}
	}

	private void Update()
	{
		//if (player.lastHeight - 5f - Camera.main.orthographicSize < player.lastHeight)
		//if (lastMapPosition - mapPosList[mapPosList.Count - 1].mapSet.height < player.lastHeight)
		firstMapPosition = mapPosList[0].mapSet.transform.position.y;

		if (firstMapPosition+ mapPosList[0].mapSet.height < player.lastHeight - 5f - Camera.main.orthographicSize)
		{
			mapPosList[0].Remove();
			mapPosList.Remove(mapPosList[0]);

			AddMapPos();
		}
	}

	void AddMapPos()
	{
		MapPos mapPos = new MapPos();
		mapPos.index = mapCount;
		mapPos.mapSet = GetRandomMapSet();
		mapPos.mapSet.transform.position = new Vector2(0f, lastMapPosition);
		mapPosList.Add(mapPos);

		lastMapHeight = mapPos.mapSet.height;
		lastMapPosition += lastMapHeight;
		mapCount++;
	}

	MapSet GetRandomMapSet()
	{
		int level = GameManager.Instance.level;
		if (level > 2)
			level = 2;

		float rnd = Random.Range(0f, 100f);
		if(rnd < mapLevRandomPercent)
		{
			level = Random.Range(1, level);
		}

		List<string> mapTagList = null;

		switch(level)
		{
			case 1:
				mapTagList = mapTagList_1;
				break;
			case 2:
				mapTagList = mapTagList_2;
				break;
		}

		int idx = Random.Range(0, mapTagList.Count);
		string mapTag = string.Format("Map_{0}_{1}", level, idx + 1);

		return PoolManager.Instance.Spawn(mapTag, Vector3.zero, Quaternion.identity).GetComponent<MapSet>();
	}
}
