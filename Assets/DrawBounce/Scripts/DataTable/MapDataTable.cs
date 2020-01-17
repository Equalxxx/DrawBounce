using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public int index;
    public string tag;
    public int mapNumber;
}

public class MapDataTable : ScriptableObject
{
    public List<MapData> mapDataList = new List<MapData>();
	public Dictionary<int, List<MapData>> mapDataDic = new Dictionary<int, List<MapData>>();

	public List<MapData> GetMapDataList(int mapNumber)
    {
		if (mapDataDic.ContainsKey(mapNumber))
		{
			return mapDataDic[mapNumber];
		}
		else
		{
			List<MapData> dataList = new List<MapData>();
			foreach (MapData mapData in mapDataList)
			{
				if (mapData.mapNumber == mapNumber)
				{
					dataList.Add(mapData);
				}
			}

			mapDataDic.Add(mapNumber, dataList);
			return mapDataDic[mapNumber];
		}
    }

	public MapData GetRandomMapData(int level)
	{
		List<MapData> mapDatas = GetMapDataList(level);
		int rnd = Random.Range(0, mapDatas.Count);

		return mapDatas[rnd];
	}
}
