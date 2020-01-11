using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MysticLights
{
    public class LoadingManager : MonoBehaviour
    {
        void Start()
        {
            StartCoroutine(LoadAsyncScene());
        }

        IEnumerator LoadAsyncScene()
        {
            AsyncOperation async = SceneManager.LoadSceneAsync(SceneControl.nextScene);

            while (!async.isDone)
            {
                yield return null;
            }
        }
    }
}
