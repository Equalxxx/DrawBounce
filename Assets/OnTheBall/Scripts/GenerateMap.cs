using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    public GameObject blockPrefab;
    public int blockCount = 30;
    public float blockSpacing = 10f;

    void Start()
    {
        Vector3 pos = Vector3.zero;
        float width = 2.8f;
        for(int i = 0; i < blockCount; i++)
        {
            int rnd = Random.Range(0, 2);
            if(rnd == 0)
            {
                pos.x = -width;
            }
            else
            {
                pos.x = width;
            }

            pos.y += blockSpacing;
            Instantiate(blockPrefab, pos, Quaternion.identity, transform);
        }
    }
}
