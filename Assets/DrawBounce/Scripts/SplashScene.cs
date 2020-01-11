using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MysticLights;

public class SplashScene : MonoBehaviour
{
    public float duration = 2f;
    public string nextSceneName;

    IEnumerator Start()
    {
        yield return FadeScreen.Instance.Fade(false);

        yield return new WaitForSeconds(duration);

        yield return FadeScreen.Instance.Fade(true);

        SceneControl.LoadNextScene(nextSceneName);
    }
}
