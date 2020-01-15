using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameLevel
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
	public List<GameLevel> gameLevelList = new List<GameLevel>();
	public List<ShopInfo> shopInfoList = new List<ShopInfo>();

	public GameLevel GetGameLevelInfo(int level)
	{
		return gameLevelList.Find(x => x.level == level);
	}

	public GameLevel GetGameLevelInfo(float height)
	{
		return gameLevelList.Find(x => x.targetHeight >= height);
	}

	public ShopInfo GetShopInfo(ShopItemType itemType)
	{
		return shopInfoList.Find(x => x.itemType == itemType);
	}
}
