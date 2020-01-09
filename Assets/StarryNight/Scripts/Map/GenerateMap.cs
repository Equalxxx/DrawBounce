using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JoyconFramework;

[System.Serializable]
public class MapPos
{
	public int index;
	public Vector2 position;
	public MapData mapData;
	public GameObject blockObject;
	public float spacing;

	public void Show(bool show)
	{
		if (mapData == null || string.IsNullOrEmpty(mapData.tag))
			return;

		if(show)
		{
			if (!blockObject)
			{
				blockObject = PoolManager.Instance.Spawn(mapData.tag, position, Quaternion.identity);
			}

			if (blockObject.activeSelf != show)
			{
				blockObject.SetActive(show);
			}
		}
		else
		{
			if (blockObject)
			{
				if (blockObject.activeSelf != show)
				{
					blockObject.SetActive(show);
				}

				blockObject = null;
			}
		}
	}
}
public class GenerateMap : MonoBehaviour
{
	public Transform target;
	public Vector3 targetPos;
	public float minSpacing = 5f;
	public float maxSpacing = 10f;

	public List<MapPos> mapPosList;
	public int maxMapCount = 10;
	public int mapCount;

	public Transform endBlockTrans;
	private Vector2 endBlockOriginPos;
	public Vector2 endBlockOffset;

	public MapDataTable mapDataTable;

	private void Start()
	{
		endBlockOriginPos = endBlockTrans.position;
	}

	public void InitMap()
	{
		mapPosList.Clear();

		for (int i = 0; i < 3; i++)
		{
			MapPos mapPos = new MapPos();
			mapPos.index = i;
			if (i != 0)
			{
				mapPos.mapData = mapDataTable.GetRandomMapData(0);

				mapPos.spacing = Random.Range(minSpacing, maxSpacing);
				int rnd = Random.Range(0, 2);

				if (rnd == 0)
				{
					mapPos.position = new Vector2(mapPos.mapData.width, i * mapPos.spacing);
				}
				else
				{
					mapPos.position = new Vector2(-mapPos.mapData.width, i * mapPos.spacing);
				}
			}

			mapPosList.Add(mapPos);
		}

		mapCount = mapPosList.Count;

		endBlockTrans.position = endBlockOriginPos;
	}

	private void Update()
	{
		targetPos = target.position;

		if (targetPos.y <= 0f)
			return;

		if(mapPosList.Count > maxMapCount)
		{
			mapPosList.Remove(mapPosList[0]);
			Vector2 pos = new Vector2(0f, mapPosList[0].position.y + (-mapPosList[0].mapData.height / 2f));
			endBlockTrans.position = pos;
		}

		if (mapPosList.Exists(x => x.position.y - minSpacing <= targetPos.y && x.position.y >= targetPos.y))
		{
			for (int i = 0; i < mapPosList.Count; i++)
			{
				if (mapPosList[i].position.y - minSpacing <= targetPos.y && mapPosList[i].position.y + minSpacing >= targetPos.y)
				{
					mapPosList[i].Show(true);
				}
				else
				{
					mapPosList[i].Show(false);
				}
			}
		}
		else
		{
			MapPos mapPos = new MapPos();
			mapPos.index = mapCount;
			mapPos.mapData = mapDataTable.GetRandomMapData(0);

			mapPos.spacing = Random.Range(minSpacing, maxSpacing);
			int rnd = Random.Range(0, 2);

			if (rnd == 0)
			{
				mapPos.position = new Vector2(mapPos.mapData.width, mapCount * mapPos.spacing);
			}
			else
			{
				mapPos.position = new Vector2(-mapPos.mapData.width, mapCount * mapPos.spacing);
			}

			mapPosList.Add(mapPos);
			mapCount++;
		}
	}
}
