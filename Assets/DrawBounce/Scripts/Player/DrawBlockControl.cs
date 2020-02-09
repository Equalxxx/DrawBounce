using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLFramework;

public class DrawBlockControl : MonoBehaviour
{
    public DrawBlock drawBlock;

    public List<Vector2> rangePositionList;
    public float addPositionDelay;
    public float minDistance = 0.1f;
    public bool isAddPosition;

    private void Awake()
    {
        PoolManager.Instance.PrepareAssets("PointerEffect");
        PoolManager.Instance.PrepareAssets("EdgeCollider");
	}

    private void Update()
    {
		if (GameManager.IsPause)
			return;
        if (GameManager.Instance.gameState != GameState.GamePlay)
            return;

        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0))
        {
			drawBlock = PoolManager.Instance.Spawn("PointerEffect", pos, Quaternion.identity).GetComponent<DrawBlock>();
            isAddPosition = true;
        }

        if(drawBlock != null)
        {
            drawBlock.myTransform.position = pos;
            drawBlock.AddPosition(pos);
        }

        if(Input.GetMouseButtonUp(0))
        {
            isAddPosition = false;
            drawBlock = null;
        }
    }
}
