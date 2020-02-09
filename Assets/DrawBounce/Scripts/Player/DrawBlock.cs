using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLFramework;

public class DrawBlock : MonoBehaviour, IPoolObject
{
    private EdgeCollider2D edgeCol2D;
    public Transform myTransform;

    public List<Vector2> positionList;
    public float duration = 1f;
    public float minDistance = 0.1f;

    private bool isDraw;

    private void Awake()
    {
        myTransform = transform;
    }

	private void OnEnable()
	{
		GameManager.PauseAction += GamePause;

		edgeCol2D = PoolManager.Instance.Spawn("EdgeCollider", Vector3.zero, Quaternion.identity).GetComponent<EdgeCollider2D>();
		edgeCol2D.enabled = false;

		isDraw = true;
		StartCoroutine(AutoDisableDraw());
	}

	private void OnDisable()
	{
		GameManager.PauseAction -= GamePause;

		if (edgeCol2D)
		{
			edgeCol2D.gameObject.SetActive(false);
			edgeCol2D = null;
		}

		positionList.Clear();
	}

	private void Update()
    {
        if(edgeCol2D.pointCount != positionList.Count)
        {
            SetPoints();
        }
    }

	void GamePause()
	{
		if (GameManager.IsPause)
		{
			gameObject.SetActive(false);
			positionList.Clear();
		}
	}

    public void AddPosition(Vector2 addPos)
    {
        if (!isDraw)
            return;

        if (positionList.Count > 0)
        {
            float dist = Vector2.Distance(positionList[positionList.Count - 1], addPos);
            if (dist > minDistance)
            {
                positionList.Add(addPos);
            }
        }
        else
        {
            positionList.Add(addPos);
        }

        Invoke("RemovePosition", 1f);
    }

    void SetPoints()
    {
		if (positionList.Count <= 1)
			return;

        Vector2[] points = new Vector2[positionList.Count];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = positionList[i];
        }

		if (!edgeCol2D.enabled)
			edgeCol2D.enabled = true;

		edgeCol2D.points = points;
    }

    void RemovePosition()
    {
        if(positionList.Count > 0)
            positionList.RemoveAt(0);
    }

    IEnumerator AutoDisableDraw()
    {
        yield return new WaitForSeconds(duration);

        isDraw = false;
    }

	public void OnSpawnObject()
	{

	}
}
