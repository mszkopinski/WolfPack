using System.Collections;
using TMPro;
using UnityEngine;
using Zenject;

namespace Wolfpack
{
    public class TitleText : MonoBehaviour
    {
        Animator animator;
        TextMeshProUGUI text;
        [Inject] GameState gameState;

        void Awake()
        {
            animator = GetComponent<Animator>();
            text = GetComponent<TextMeshProUGUI>();
        }
    
        void Start()
        {
            gameState.StateChanged += OnStateChanged;
        }

        void OnStateChanged()
        {
            var status = gameState.Value.Status;
            Debug.Log(status);
            
            if (status == GameStatus.Menu)
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