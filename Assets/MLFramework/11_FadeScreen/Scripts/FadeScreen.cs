using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MLFramework
{
    public class FadeScreen : Singleton<FadeScreen>
    {
        private Image fadeImage;
        public float duration = 1f;
        public bool isFade;

        private void Awake()
        {
            fadeImage = GetComponentInChildren<Image>();
            fadeImage.enabled = true;
        }

        public bool StartFade(bool fade)
        {
            if (isFade)
                return false;

            StartCoroutine(Fade(fade));

            return true;
        }

        public IEnumerator Fade(bool fade)
        {
            isFade = true;

            if (fade)
            {
                fadeImage.canvasRenderer.SetAlpha(0f);
                fadeImage.CrossFadeAlpha(1f, duration, true);
            }
            else
            {
                fadeImage.canvasRenderer.SetAlpha(1f);
                fadeImage.CrossFadeAlpha(0f, duration, true);
            }

            yield return new WaitForSeconds(duration);

            isFade = false;
        }
    }
}