using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Wolfpack
{
    [RequireComponent(typeof(Image))]
    public class FadeInImage : MonoSingleton<FadeInImage>
    { 
        [SerializeField] float fadeDuration = 3f;

        Image image;

        void Awake()
        {
            image = GetComponent<Image>();
        }

        public void Fade(FadeDirection fadeDirection, float duration)
        {
            var color = image.color;

            switch (fadeDirection)
            {
                case FadeDirection.In:
                    color.a = 0f;
                    image.color = color;
                    StartCoroutine(FadeInOut(fadeDirection, duration));
                    break;
                case FadeDirection.Out:
                    color.a = 1f;
                    image.color = color;
                    StartCoroutine(FadeInOut(fadeDirection, duration));
                    break;
            }
        }

        IEnumerator FadeInOut(FadeDirection fadeDirection, float duration)
        {
            var elapsedTime = 0.0f;
            var color = image.color;
            while (elapsedTime < duration)
            {
                yield return null;
                elapsedTime += Time.deltaTime ;
                color.a = fadeDirection == FadeDirection.Out 
                    ? 1.0f - Mathf.Clamp01(elapsedTime / duration) 
                    : 0.0f + Mathf.Clamp01(elapsedTime / duration);
                image.color = color;
            }
        }
    }
}