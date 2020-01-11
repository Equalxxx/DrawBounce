using UnityEngine;

namespace MysticLights
{ 
    public class Singleton_Prefab<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T s_Instance;
    
        public static T Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = (T)FindObjectOfType(typeof(T));

                    if (s_Instance == null)
                    {
                        //string resourcePath = string.Format("Manager/{0}", typeof(T));
                        var singletonObject = Instantiate(ResourceManager.LoadAsset<GameObject>("manager",typeof(T).ToString()));
                        s_Instance = singletonObject.GetComponent<T>();
                        singletonObject.name = typeof(T).ToString();
                    }
                }

                return s_Instance;
            }
        }
    }
}