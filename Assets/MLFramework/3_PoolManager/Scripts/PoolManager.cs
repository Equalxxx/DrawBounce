using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

namespace MysticLights
{
    public class PoolManager : Singleton<PoolManager>
    {
        public ResourceManager.LinkType resLinkType = ResourceManager.LinkType.Resources;
        private Dictionary<string, List<GameObject>> poolDic = new Dictionary<string, List<GameObject>>();
        private Dictionary<string, GameObject> prefabDic = new Dictionary<string, GameObject>();
        private List<Transform> parentList = new List<Transform>();

        //Pool Data
        private string tablePath = "datatables";
        public PoolTable poolTable;

        private void Awake()
        {
            if (poolTable == null)
            {
                poolTable = ResourceManager.LoadAsset<PoolTable>(tablePath, "PoolTable", resLinkType);
                if (poolTable == null)
                {
                    Debug.LogError("Not found PoolTable");
                    return;
                }
            }
        }

        ///<summary>
        ///Prepare Pool Asset
        ///</summary>
        public void PrepareAssets(string tag, Transform parentTrans = null)
        {
            if (poolDic.ContainsKey(tag))
                return;

            PoolTable.PoolInfo poolInfo = poolTable.GetPoolInfo(tag);
            if (poolInfo == null)
            {
                Debug.LogError("Prepare Faild. Not found pool info : " + tag);
                return;
            }

            for (int i = 0; i < poolInfo.preloadCount; i++)
            {
                GameObject newObj = CreatePoolObject(poolInfo.tag, parentTrans);
                if (newObj == null)
                {
                    Debug.LogError("Prepare Faild. Not found PoolObject : " + tag);
                    return;
                }
            }

            Debug.Log("Prepare Success PoolObject : " + tag);
        }

        private void OnDestroy()
        {
            //foreach(KeyValuePair<string, GameObject> keyPair in prefabDic)
            //{
            //    PoolTable.PoolInfo poolInfo = poolTable.GetPoolInfo(keyPair.Key);
            //    ResourceManager.Release(poolInfo.path);
            //}

            ResourceManager.ReleaseAll();
        }

        ///<summary>
        ///Spawn PoolObject
        ///</summary>
        public GameObject Spawn(string tag, Vector3 spawnPos, Quaternion spawnRot, Transform parentTrans = null)
        {
            GameObject spawnObj = GetPoolObject(tag, parentTrans);
            if (spawnObj == null)
            {
                Debug.LogError("Not found GameObject : " + tag);
                return null;
            }

            spawnObj.SetActive(true);
            spawnObj.transform.position = spawnPos;
            spawnObj.transform.rotation = spawnRot;

            IPoolObject poolObj = spawnObj.GetComponent<IPoolObject>();
            if (poolObj == null)
            {
                Debug.LogError("Not found IPoolObject : " + tag);
                return null;
            }

            poolObj.OnSpawnObject();

            return spawnObj;
        }

        GameObject GetPoolObject(string tag, Transform parentTrans = null)
        {
            List<GameObject> poolList = GetPoolList(tag);

            GameObject spawnObj = poolList.Find(x => !x.activeSelf);
            if (spawnObj == null)
            {
                spawnObj = CreatePoolObject(tag, parentTrans);
            }

            return spawnObj;
        }

        GameObject CreatePoolObject(string tag, Transform parentTrans = null)
        {
            List<GameObject> poolList = GetPoolList(tag);

            //Resources 경로 설정
            PoolTable.PoolInfo poolInfo = poolTable.GetPoolInfo(tag);
            if (poolInfo == null)
            {
                Debug.LogError("Not found PoolInfo : " + tag);
                return null;
            }

            if (!prefabDic.ContainsKey(tag))
            {
                GameObject newPrefab = ResourceManager.LoadAsset<GameObject>(poolInfo.path, poolInfo.tag, resLinkType);
                prefabDic.Add(poolInfo.tag, newPrefab);
            }

            GameObject newObj = Instantiate(prefabDic[tag]);

            newObj.SetActive(false);

            newObj.name = string.Format("{0} ({1})", tag, poolList.Count);

            if (parentTrans == null)
            {
                string parentName = string.Format("{0}Parent", tag);

                Transform _parentTrans = parentList.Find(x => x.name.Equals(parentName));
                if (_parentTrans == null)
                {
                    _parentTrans = new GameObject().GetComponent<Transform>();
                    _parentTrans.parent = this.transform;
                    _parentTrans.name = parentName;
                    parentList.Add(_parentTrans);
                }

                newObj.transform.parent = _parentTrans;
            }
            else
            {
                newObj.transform.parent = parentTrans;
            }

            poolList.Add(newObj);

            return newObj;
        }

        List<GameObject> GetPoolList(string tag)
        {
            if (!poolDic.ContainsKey(tag))
            {
                //poolDic 에 추가
                List<GameObject> newPoolObjectList = new List<GameObject>();

                poolDic.Add(tag, newPoolObjectList);
            }

            return poolDic[tag];
        }
    }
}
