using System.Collections;
using UnityEngine;

namespace Wolfpack
{
    public class CoroutineRunner : MonoSingleton<CoroutineRunner>
    {
        public void Run(IEnumerator coroutine, float? delay = null)
        {
            if (delay.HasValue)
            {
                StartCoroutine(RunWithDelay(coroutine, delay.Value));
                return;
            }

            StartCoroutine(coroutine);
        }

        IEnumerator RunWithDelay(IEnumerator coroutine, float delay)
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(coroutine);
        }
    }
}