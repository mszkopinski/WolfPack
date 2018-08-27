using System.Collections;
using TMPro;
using UnityEngine;

namespace Wolfpack
{
    public class TitleText : MonoBehaviour
    {
        Animator animator;
        TextMeshProUGUI text;

        void Awake()
        {
            animator = GetComponent<Animator>();
            text = GetComponent<TextMeshProUGUI>();
        }
    
        void Start()
        {
            GameManager.StateChanged += OnStateChanged;
        }

        void OnStateChanged(GameState gameState)
        {
            if (gameState == GameState.Menu)
                StartCoroutine(FadeOut(.5f));
        }
    
        IEnumerator FadeOut(float duration)
        {
            var elapsedTime = 0.0f;
            var color = text.color;
            var outline = text.outlineWidth;
            while (elapsedTime < duration)
            {
                yield return null;
                elapsedTime += Time.deltaTime ;
                color.a = 1.0f - Mathf.Clamp01(elapsedTime / duration);
                outline = 0.0f + Mathf.Clamp01(elapsedTime / duration);
                text.color = color;
                text.outlineWidth = outline;
            }
        }
    }
}