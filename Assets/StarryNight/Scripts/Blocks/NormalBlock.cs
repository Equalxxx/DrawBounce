using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalBlock : DefaultBlock
{
	public float width = 3f;

	public override void OnSpawnObject()
	{
		int rnd = Random.Range(0, 2);
		Vector3 pos = transform.position;

		if (rnd == 0)
		{
			pos.x = width;
		}
		else
		{
			pos.x = -width;
		}

		transform.position = pos;
	}
}
