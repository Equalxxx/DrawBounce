using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopScrollView : MonoBehaviour
{
	public ShopTapType shopTapType;
	
	public void RefreshUI(ShopTapType tapType)
	{
		if(shopTapType == tapType)
		{
			gameObject.SetActive(true);
		}
		else
		{
			gameObject.SetActive(false);
		}
	}
}
