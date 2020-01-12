using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public int index;
    public string tag;
    public int level;
}

public class MapDataTable : ScriptableObject
{
    public List<MapData> mapDataList = new List<MapData>();

    public List<MapData> GetMapDataList(int level)
    {
        List<MapData> dataList = new List<MapData>();

        foreach(MapData mapData in mapDataList)
        {
            if(mapData.level == level)
            {
                dataList.Add(mapData);
            }
		}

        return dataList;
    }

	public MapData GetRandomMapData(int level)
	{
		List<MapData> mapDatas = GetMapDataList(level);
		int rnd = Random.Range(0, mapDatas.Count);

		return mapDatas[rnd];
	}
}
