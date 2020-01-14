using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIGroup : UIGroup
{
	private void OnValidate()
	{
		groupType = UIGroupType.Game;
	}
}
