using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Wolfpack
{
    [RequireComponent(typeof(Image))]
    public class ScreenFading : MonoSingleton<ScreenFading>
    { 
        [SerializeField] float fadeDuration = 3f;

        Image image;
        IEnumerator fadeCoroutine;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        public IEnumerator FadeInOut(FadeDirection fadeDirection, float duration)
        {
            image.color.SetColorAlpha(fadeDirection == FadeDirection.In ? 0f : 1f);
            var elapsedTime = 0.0f;
            var color = image.color;
            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime ;
                color.a = fadeDirection == FadeDirection.Out 
                    ? 1.0f - Mathf.Clamp01(elapsedTime / duration) 
                    : 0.0f + Mathf.Clamp01(elapsedTime / duration);
                image.color = color;
                yield return null;
            }
            fadeCoroutine = null;
        }
    }
}