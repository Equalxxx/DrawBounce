using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoyconFramework;

public class GenerateMap : MonoBehaviour
{
    public int mapLevel;

    public int blockCount = 30;
    public float blockSpacing = 10f;

    public List<GameObject> blockList;
    public MapDataTable mapDataTable;

    private void Awake()
    {
        mapDataTable = ResourceManager.LoadAsset<MapDataTable>("DataTables", "MapDataTable", ResourceManager.LinkType.Resources);

        for (int i = 0; i < mapDataTable.mapDataList.Count; i++)
        {
            PoolManager.Instance.PrepareAssets(mapDataTable.mapDataList[i].tag);
        }
    }

	public void Generate()
    {
        Vector3 pos = Vector3.zero;
        float width = 2.8f;

        for (int i = 0; i < blockCount; i++)
        {
            int rnd = Random.Range(0, 2);
            if (rnd == 0)
            {
                pos.x = -width;
            }
            else
            {
                pos.x = width;
            }

            int rndProbIndex = Random.Range(0, 2);

            pos.y += mapDataTable.mapDataList[rndProbIndex].spacing;


            GameObject newBlock = PoolManager.Instance.Spawn(mapDataTable.mapDataList[rndProbIndex].tag, pos, Quaternion.identity);

            blockList.Add(newBlock);
        }
    }

    public void RemoveBlocks()
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            blockList[i].SetActive(false);
        }

        blockList.Clear();
    }
}
