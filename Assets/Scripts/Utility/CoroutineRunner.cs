using System;
using System.Collections;
using UnityEngine;

namespace Wolfpack
{
    public class CoroutineRunner : MonoSingleton<CoroutineRunner>
    {
        public void Run(IEnumerator coroutine, float? delay = null, Action callback = null)
        {
            if (delay.HasValue)
            {
                StartCoroutine(RunWithDelay(coroutine, delay.Value, callback));
                return;
            }

            StartCoroutine(coroutine);
        }

        public void Run(IEnumerator coroutine, Action callback)
        {
            StartCoroutine(RunWithCallback(coroutine, callback));
        }

        IEnumerator RunWithDelay(IEnumerator coroutine, float delay, Action callback)
        {
            yield return new WaitForSeconds(delay);
            yield return StartCoroutine(coroutine);
            callback?.Invoke();
        }
        
        IEnumerator RunWithCallback(IEnumerator coroutine, Action callback)
        {
            yield return StartCoroutine(coroutine);
            callback?.Invoke();
        }
    }
}