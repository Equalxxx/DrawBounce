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

	public void Show(bool show)
	{
		if (mapData == null)
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
	public float spacing = 10f;
	public List<MapPos> mapPosList;

	public MapDataTable mapDataTable;

	private void OnValidate()
	{
		for (int i = 0; i < 3; i++)
		{
			MapPos mapPos = new MapPos();
			mapPos.index = i;
			mapPos.position = new Vector2(0f, i * spacing);
			if (i != 0)
				mapPos.mapData = mapDataTable.GetRandomMapData(0);

			mapPosList.Add(mapPos);
		}
	}

	private void Update()
	{
		targetPos = target.position;

		if (targetPos.y <= 0f)
			return;

		if (mapPosList.Exists(x => x.position.y - spacing <= targetPos.y && x.position.y >= targetPos.y))
		{
			for (int i = 0; i < mapPosList.Count; i++)
			{
				if (mapPosList[i].position.y - spacing <= targetPos.y && mapPosList[i].position.y + spacing >= targetPos.y)
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
			mapPos.index = mapPosList.Count;
			mapPos.position = new Vector2(0f, mapPosList.Count * spacing);
			mapPos.mapData = mapDataTable.GetRandomMapData(0);

			mapPosList.Add(mapPos);
		}
	}
}
