using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameLevelInfo
{
	[HideInInspector]
	public int index;

	public int level;
	public int mapNumber;
}

[System.Serializable]
public class TargetHeightInfo
{
	[HideInInspector]
    public int index;

    public int level;
	public float targetHeight;
}

public enum ShopItemType { HP, Height }

[System.Serializable]
public class ShopInfo
{
	[HideInInspector]
	public int index;

	public ShopItemType itemType;
	public int price;
}

public class GameDataTable : ScriptableObject
{
	public List<GameLevelInfo> gameLevelInfoList = new List<GameLevelInfo>();
	public Dictionary<int, List<GameLevelInfo>> gameLevelDic = new Dictionary<int, List<GameLevelInfo>>();

	public List<TargetHeightInfo> targetHeightList = new List<TargetHeightInfo>();
	public List<ShopInfo> shopInfoList = new List<ShopInfo>();

	public List<GameLevelInfo> GetGameLevelInfo(int level)
	{
		if (gameLevelDic.Count == 0)
		{
			for (int i = 0; i < gameLevelInfoList.Count; i++)
			{
				if(!gameLevelDic.ContainsKey(gameLevelInfoList[i].level))
				{
					List<GameLevelInfo> infoList = new List<GameLevelInfo>();
					infoList.Add(gameLevelInfoList[i]);
					gameLevelDic.Add(gameLevelInfoList[i].level, infoList);
				}
				else
				{
					List<GameLevelInfo> infoList = gameLevelDic[gameLevelInfoList[i].level];
					infoList.Add(gameLevelInfoList[i]);
				}
			}
		}

		return gameLevelDic[level];
	}

	public TargetHeightInfo GetTargetHeightInfo(int level)
	{
		TargetHeightInfo heightInfo = targetHeightList.Find(x => x.level == level);
		if (heightInfo == null)
			heightInfo = targetHeightList[targetHeightList.Count - 1];

		return heightInfo;
	}

	public TargetHeightInfo GetTargetHeightInfo(float height)
	{
		TargetHeightInfo heightInfo = targetHeightList.Find(x => x.targetHeight >= height);
		if (heightInfo == null)
			heightInfo = targetHeightList[targetHeightList.Count - 1];

		return heightInfo;
	}

	public ShopInfo GetShopInfo(ShopItemType itemType)
	{
		return shopInfoList.Find(x => x.itemType == itemType);
	}
}
