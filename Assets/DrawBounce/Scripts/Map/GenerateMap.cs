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
	public float lastMapPosition = 0f;
	public float startMapHeight = 5f;
	public float firstMapPosition;

	public int mapCount;

	public float mapLevRandomPercent = 50f;

	public List<MapPos> mapPosList;

	private PlayableBlock player;

	public bool isGenerate;

	private void Start()
	{
		PrepareAssets();
	}

	void PrepareAssets()
	{
		foreach (KeyValuePair<int, List<MapData>> keyPair in GameManager.Instance.mapDataTable.mapDataDic)
		{
			for (int i = 0; i < keyPair.Value.Count; i++)
			{
				PoolManager.Instance.PrepareAssets(keyPair.Value[i].tag);
			}
		}
	}

	private void OnEnable()
	{
		GameManager.GamePlayAction += InitMap;
		GameManager.GameOverAction += StopGenerate;
		PlayableBlock.MoveToAction += StartGenerate;
	}

	private void OnDisable()
	{
		GameManager.GamePlayAction -= InitMap;
		GameManager.GameOverAction -= StopGenerate;
		PlayableBlock.MoveToAction -= StartGenerate;
	}

	void InitMap()
	{
		player = GameManager.Instance.player;

		if (GameManager.Instance.gameInfo.startHeight >= 100f)
		{
			lastMapPosition = GameManager.Instance.gameInfo.startHeight;
			StopGenerate();
		}
		else
		{
			lastMapPosition = startMapHeight;
		}

		lastMapHeight = 0f;
		mapCount = 0;

		for (int i = 0; i < mapPosList.Count; i++)
		{
			mapPosList[i].Remove();
		}

		mapPosList.Clear();

		if (mapPosList.Count <= 0)
		{
			for (int i = 0; i < 4; i++)
			{
				AddMapPos();
			}
		}
	}

	void StartGenerate()
	{
		isGenerate = true;
	}

	void StopGenerate()
	{
		isGenerate = false;
	}

	private void Update()
	{
		if (!isGenerate)
			return;

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
		mapPos.mapSet.transform.position = new Vector2(0f, lastMapPosition + startMapHeight);
		mapPosList.Add(mapPos);

		lastMapHeight = mapPos.mapSet.height;
		lastMapPosition += lastMapHeight;
		mapCount++;
	}

	MapSet GetRandomMapSet()
	{
		int level = GameManager.Instance.curTargetHeight.level;
		if (level > GameManager.Instance.maxLevel)
			level = GameManager.Instance.maxLevel;

		int mapNumber = GetRandomMapNumber(level);

		List<MapData> mapDataList = GameManager.Instance.mapDataTable.GetMapDataList(mapNumber);

		int rnd = Random.Range(0, mapDataList.Count);

		return PoolManager.Instance.Spawn(mapDataList[rnd].tag, Vector3.zero, Quaternion.identity).GetComponent<MapSet>();
	}

	int GetRandomMapNumber(int lev)
	{
		float rnd = 0f;

		List<GameLevelInfo> gameLevelInfo = GameManager.Instance.gameDataTable.GetGameLevelInfo(lev);

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
