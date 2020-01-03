using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
	public Transform[] bgTrans;
	public Transform cameraTrans;

	public float height = 5f;
	public float size = 10f;

	void Update()
    {
		for (int i = 0; i < bgTrans.Length; i++)
		{
			float dist = cameraTrans.position.y - bgTrans[i].position.y;

			if(Mathf.Abs(dist) > size)
			{
				Vector3 pos = bgTrans[i].position;

				if(dist > 0)
				{
					pos.y = cameraTrans.position.y + size;
				}
				else
				{
					pos.y = cameraTrans.position.y - size;
				}

				bgTrans[i].position = pos;
			}
		}
    }
}
