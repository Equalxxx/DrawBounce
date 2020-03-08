using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLFramework;

public class SplashManager : MonoBehaviour
{
    public float duration = 2f;
    public string nextSceneName;
	public float signInTimeOut = 10f;

    IEnumerator Start()
    {
        yield return FadeScreen.Instance.Fade(false);

        yield return new WaitForSeconds(duration);

		GooglePlayManager.Instance.SignIn();

		float t = 0f;
		while (!GooglePlayManager.IsSignInProcess)
		{
			t += Time.deltaTime / signInTimeOut;
			if (t >= 1f)
			{
				Debug.LogWarning("SignIn Time Out");
				break;
			}

			yield return null;
		}

		yield return new WaitForSeconds(1f);

        yield return FadeScreen.Instance.Fade(true);

        SceneControl.LoadNextScene(nextSceneName);
    }
}
