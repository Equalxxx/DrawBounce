using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{
    public GameObject[] blockPrefab;
    public int blockCount = 30;
    public float blockSpacing = 10f;

    public List<GameObject> blockList;

    public void Generate()
    {
        Vector3 pos = Vector3.zero;
        float width = 2.8f;
        for (int i = 0; i < blockCount; i++)
        {
            int rnd = Random.Range(0, 2);
            if (rnd == 0)
            {
                pos.x = -width;
            }
            else
            {
                pos.x = width;
            }

            pos.y += blockSpacing;

            int rndProbIndex = Random.Range(0, blockPrefab.Length);
            GameObject newBlock = Instantiate(blockPrefab[rndProbIndex], pos, Quaternion.identity, transform);

            blockList.Add(newBlock);
        }
    }

    public void RemoveBlocks()
    {
        for (int i = 0; i < blockList.Count; i++)
        {
            Destroy(blockList[i]);
        }

        blockList.Clear();
    }
}
