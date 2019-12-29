using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBlock : MonoBehaviour
{
    public GameObject edgeColPrefab;
    private EdgeCollider2D edgeCol2D;
    public Transform myTransform;

    public List<Vector2> positionList;
    public float duration = 1f;
    public float minDistance = 0.1f;

    private bool isDraw;

    private void Awake()
    {
        myTransform = transform;
        edgeCol2D = Instantiate(edgeColPrefab, Vector3.zero, Quaternion.identity).GetComponent<EdgeCollider2D>();

        isDraw = true;
        StartCoroutine(AutoDisableDraw());
    }

    private void Update()
    {
        if(edgeCol2D.pointCount != positionList.Count)
        {
            SetPoints();
        }
    }

    private void OnDestroy()
    {
        Destroy(edgeCol2D.gameObject);
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
        Vector2[] points = new Vector2[positionList.Count];

        for (int i = 0; i < points.Length; i++)
        {
            points[i] = positionList[i];
        }

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
}
