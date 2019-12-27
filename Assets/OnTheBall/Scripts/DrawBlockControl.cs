using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawBlockControl : MonoBehaviour
{
    public GameObject pointerEffectPrefab;
    public DrawBlock drawBlock;

    public List<Vector2> rangePositionList;
    public float addPositionDelay;
    public float minDistance = 0.1f;
    public bool isAddPosition;

    //private void Start()
    //{
    //    StartCoroutine(AddPositions());
    //}

    private void Update()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if(Input.GetMouseButtonDown(0))
        {
            drawBlock = Instantiate(pointerEffectPrefab, pos, Quaternion.identity).GetComponent<DrawBlock>();
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

    //IEnumerator AddPositions()
    //{
    //    while(true)
    //    {
    //        if(isAddPosition)
    //        {
    //            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //            if (rangePositionList.Count > 0)
    //            {
    //                float dist = Vector2.Distance(rangePositionList[rangePositionList.Count - 1], pos);
    //                if (dist > minDistance)
    //                {
    //                    rangePositionList.Add(pos);
    //                    Invoke("RemovePosition", 1f);
    //                }
    //            }
    //            else
    //            {
    //                rangePositionList.Add(pos);
    //                Invoke("RemovePosition", 1f);
    //            }
    //        }

    //        yield return null;
    //    }
    //}

    //void RemovePosition()
    //{
    //    if(rangePositionList.Count > 0)
    //    {
    //        rangePositionList.RemoveAt(0);
    //    }
    //}
}
