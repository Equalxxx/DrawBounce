using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapPosition
{
	public Vector2 position;
	public string blockTag;
}

public class TestGenerateMap : MonoBehaviour
{
	public Transform playerTrans;
	public Vector2 playerPosition;
	public List<MapPosition> mapPosList;
	public MapPosition curMapPos;
	public float spacing = 10f;
	public GameObject blockPrefab;

	private void OnValidate()
	{
		mapPosList.Clear();

		for (int i = 0; i < 3; i++)
		{
			MapPosition mapPos = new MapPosition();
			mapPos.position = new Vector2(0, i * spacing);
			mapPos.blockTag = string.Format("Pos {0}", i);
			mapPosList.Add(mapPos);
		}
	}

	private void Start()
	{
		curMapPos = mapPosList[0];
	}

	private void Update()
	{
		playerPosition = playerTrans.position;
		if(CheckMapPosition())
		{
			Instantiate(blockPrefab, new Vector2(-3f, curMapPos.position.y), Quaternion.identity);
		}
	}

	bool CheckMapPosition()
	{
		if (playerPosition.y < 0)
			return false;

		if (CheckPosition(curMapPos))
			return false;

		foreach(MapPosition mapPos in mapPosList)
		{
			if(CheckPosition(mapPos))
			{
				curMapPos = mapPos;
				return true;
			}
		}

		AddMapPosition();
		return true;
	}

	void AddMapPosition()
	{
		MapPosition mapPos = new MapPosition();
		mapPos.blockTag = string.Format("Pos {0}", mapPosList.Count);
		mapPos.position = new Vector2(0, mapPosList.Count * spacing);
		mapPosList.Add(mapPos);
	}

	bool CheckPosition(MapPosition mapPos)
	{
		if (playerTrans.position.y - mapPos.position.y >= 0 &&
				playerTrans.position.y - mapPos.position.y <= spacing)
		{
			return true;
		}
		else
			return false;
	}

	//void Update()
	//{
	//	playerPosition = playerTrans.position;
	//	CheckMapPosition();
	//}

	//void CheckMapPosition()
	//{
	//	if (CheckPosition(curMapPos))
	//		return;

	//	int idx = mapPosList.Count / 2;

	//	if(CheckPosition(mapPosList[idx]))
	//	{
	//		curMapPos = mapPosList[idx];
	//		return;
	//	}

	//	if (playerPosition.y - mapPosList[idx].position.y > 0f)
	//	{
	//		CheckMapPosition(idx);
	//	}
	//	else
	//	{
	//		CheckMapPosition(idx);
	//	}
	//}

	//void BinarySearch(int index, int dir)
	//{
	//	int idx = (mapPosList.Count - index) / 2;
	//	if (CheckPosition(mapPosList[idx]))
	//	{
	//		curMapPos = mapPosList[idx];
	//	}
	//	else
	//	{
	//		if (playerPosition.y - mapPosList[idx].position.y > 0f)
	//		{

	//		}
	//		else
	//		{

	//		}
	//	}
	//}
}
