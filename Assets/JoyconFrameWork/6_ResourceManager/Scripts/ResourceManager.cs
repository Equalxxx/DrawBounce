using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JoyconFramework
{
    public static class ResourceManager
    {
        public enum LinkType { AssetBundle, Resources }

        private static Dictionary<string, AssetBundle> AssetBundleCache = new Dictionary<string, AssetBundle>();
        private static bool isLoadBundle;

        public delegate void AssetBundleCallBack(AssetBundle assetBundle);

        //public static void ClearAssetBundles()
        //{
        //    AssetBundle[] assetBundles = Resources.FindObjectsOfTypeAll<AssetBundle>();
        //    if (assetBundles.Length > 0)
        //    {
        //        if (AssetBundleCache.Count > 0)
        //        {
        //            for (int i = 0; i < assetBundles.Length; i++)
        //            {
        //                if (assetBundles[i].name.Equals(""))
        //                    continue;

        //                if (!AssetBundleCache.ContainsValue(assetBundles[i]))
        //                {
        //                    assetBundles[i].Unload(true);
        //                    assetBundles[i] = null;
        //                }
        //            }

        //            Resources.UnloadUnusedAssets();
        //        }
        //        else
        //        {
        //            ReleaseAll();
        //        }
        //    }
        //}

        public static T LoadAsset<T>(string path, string tag, LinkType linkType = LinkType.AssetBundle)
        {
            object loadAsset = null;

            switch (linkType)
            {
                case LinkType.AssetBundle:
                    GetAssetBundle(path, (AssetBundle assetBundle) =>
                    {
                        if (assetBundle != null)
                        {
                            loadAsset = assetBundle.LoadAsset(tag, typeof(T));
                        }
                    });
                    break;
                case LinkType.Resources:
                    loadAsset = Resources.Load(Path.Combine(path, tag), typeof(T));
                    break;
            }

            return (T)loadAsset;
        }

        public static IEnumerator LoadBundleAsync(string bundleName)
        {
            while (isLoadBundle)
            {
                yield return null;
            }

            if (!AssetBundleCache.ContainsKey(bundleName))
            {
                string folderPath = Path.Combine(Application.streamingAssetsPath, "AssetBundles");
                string combinePath = Path.Combine(folderPath, bundleName);

                isLoadBundle = true;
                var request = AssetBundle.LoadFromFileAsync(Path.Combine(folderPath, bundleName));
                yield return request;

                AssetBundleCache.Add(bundleName, request.assetBundle);
                isLoadBundle = false;
            }
        }

        public static IEnumerator LoadAssetAsync<T>(string path, string tag,
             Action<UnityEngine.Object> onComplete, LinkType linkType = LinkType.AssetBundle)
        {
            switch (linkType)
            {
                case LinkType.AssetBundle:
                    yield return LoadBundleAsync(path);

                    AssetBundleRequest abRequest = AssetBundleCache[path].LoadAssetAsync<T>(tag);
                    yield return abRequest;

                    if (onComplete != null)
                        onComplete(abRequest.asset);
                    break;
                case LinkType.Resources:
                    ResourceRequest resRequest = Resources.LoadAsync(Path.Combine(path, tag));
                    yield return resRequest;

                    if (onComplete != null)
                        onComplete(resRequest.asset);
                    break;
            }
        }

        public static void Release(string path)
        {
            if (AssetBundleCache.ContainsKey(path))
            {
                AssetBundle bundle = AssetBundleCache[path];

                Debug.Log("Release assetbundle : " + bundle.name);

                bundle.Unload(false);

                AssetBundleCache.Remove(path);
            }

            Resources.UnloadUnusedAssets();
        }

        public static void ReleaseAll()
        {
            foreach (KeyValuePair<string, AssetBundle> keyPair in AssetBundleCache)
            {
                AssetBundle bundle = keyPair.Value;
                if (bundle == null)
                    continue;

                Debug.Log("Release assetbundle : " + bundle.name);

                bundle.Unload(false);
            }

            AssetBundleCache.Clear();
            AssetBundle.UnloadAllAssetBundles(false);
            Resources.UnloadUnusedAssets();
        }

        public static void GetAssetBundle(string bundle, AssetBundleCallBack callback)
        {
            LoadAssetBundleFromCache(bundle, (AssetBundle cache) => {
                if (cache != null)
                {
                    callback(cache);
                }
                else
                {
                    LoadAssetBundleFromStreamingAssets(bundle, (AssetBundle streamingassets) => {
                        if (streamingassets != null)
                        {
                            callback(streamingassets);
                        }
                        else
                        {
                            callback(null);
                        }
                    });
                }
            });
        }

        private static void LoadAssetBundleFromCache(string bundle, AssetBundleCallBack callback)
        {
            if (!string.IsNullOrEmpty(bundle))
            {
                if (callback != null)
                {
                    AssetBundle assetBundle = null;
                    if (AssetBundleCache.ContainsKey(bundle))
                    {
                        assetBundle = AssetBundleCache[bundle];
                    }

                    if (assetBundle != null)
                    {
                        callback(assetBundle);
                    }
                    else
                    {
                        callback(null);
                    }
                }
            }
        }

        private static void LoadAssetBundleFromStreamingAssets(string bundle, AssetBundleCallBack callback)
        {
            if (!string.IsNullOrEmpty(bundle))
            {
                if (callback != null)
                {
                    AssetBundle assetBundle = null;
                    StringBuilder localPath = new StringBuilder();
                    localPath.Append(Application.streamingAssetsPath);

                    //#if UNITY_ANDROID
                    //				localPath.Append("/Android");
                    //#elif UNITY_IOS || UNITY_IPHONE
                    //				localPath.Append("/iOS");
                    //#else
                    localPath.Append("/AssetBundles");
                    //#endif
                    if (Directory.Exists(localPath.ToString()))
                    {
                        localPath.Append("/");
                        localPath.Append(bundle);
                        assetBundle = AssetBundle.LoadFromFile(localPath.ToString());
                    }
                    else
                    {
                        Debug.LogError("Not found AssetBundle path : " + localPath);
                    }

                    if (assetBundle != null)
                    {
                        AssetBundleCache.Add(bundle, assetBundle);
                        callback(assetBundle);
                    }
                    else
                    {
                        callback(null);
                    }
                }
            }
        }
    }
}