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

	public float mapLevRandomPercent = 50f;

	public List<MapPos> mapPosList;

	private Player player;

	public int maxMapLevel = 5;
	private Dictionary<int, List<MapData>> mapDataDic;
	public MapDataTable mapDataTable;

	private void Awake()
	{
		mapDataDic = new Dictionary<int, List<MapData>>();
		for (int i = 1; i <= maxMapLevel; i++)
		{
			mapDataDic.Add(i, mapDataTable.GetMapDataList(i));
		}

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
		foreach(KeyValuePair<int, List<MapData>> keyPair in mapDataDic)
		{
			for(int i = 0; i < keyPair.Value.Count; i++)
			{
				PoolManager.Instance.PrepareAssets(keyPair.Value[i].tag);
			}
		}
	}

	private void Update()
	{
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
		if (level > maxMapLevel)
			level = maxMapLevel;

		level = GetRandomMapLevel(level);

		List<MapData> mapDataList = mapDataDic[level];

		int rnd = Random.Range(0, mapDataList.Count);
		//string mapTag = string.Format("Map_{0}_{1}", level, idx + 1);

		return PoolManager.Instance.Spawn(mapDataList[rnd].tag, Vector3.zero, Quaternion.identity).GetComponent<MapSet>();
	}

	int GetRandomMapLevel(int lev)
	{
		float rnd = 0f;

		while (true)
		{
			rnd = Random.Range(0f, 100f);
			if (rnd < mapLevRandomPercent)
			{
				return lev;
			}
			else
			{
				lev--;
				if (lev <= 1)
					return 1;
			}
		}
	}
}
