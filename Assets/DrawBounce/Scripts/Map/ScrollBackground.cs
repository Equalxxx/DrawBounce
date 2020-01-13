using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
	public List<Transform> bgTrans;

	public bool isScroll;
	
	public float size = 10f;
	private PlayableBlock player;

	private void OnEnable()
	{
		GameManager.GameInitAction += InitBG;
		GameManager.SetStartHeightAction += SetStartPosition;
		PlayableBlock.MoveToAction += StartScroll;
	}

	private void OnDisable()
	{
		GameManager.GameInitAction -= InitBG;
		GameManager.SetStartHeightAction -= SetStartPosition;
		PlayableBlock.MoveToAction -= StartScroll;
	}

	void InitBG()
	{
		isScroll = false;
		player = GameManager.Instance.player;

		for (int i = 0; i < bgTrans.Count; i++)
		{
			bgTrans[i].position = new Vector3(0f, (i-1) * 10f, bgTrans[i].position.z);
		}
	}

	void StartScroll()
	{
		if(!isScroll)
			isScroll = true;
	}

	void SetStartPosition(float height)
	{
		for (int i = 0; i < bgTrans.Count; i++)
		{
			Vector3 pos = bgTrans[i].position;
			pos.y += height;
			bgTrans[i].position = pos;
		}
	}

	void FixedUpdate()
    {
		if (!isScroll)
			return;

		if(player.height > bgTrans[0].position.y + size * 2f)
		{
			SetBGPosition(1);
		}
		else if(player.height < bgTrans[0].position.y - size * 2f)
		{
			SetBGPosition(-1);
		}
    }

	void SetBGPosition(int dir)
	{
		Transform bg = bgTrans[0];
		bgTrans.Remove(bgTrans[0]);

		Vector3 pos = bg.position;
		pos.y += size * 3f * dir;
		bg.position = pos;

		bgTrans.Add(bg);
	}
}
