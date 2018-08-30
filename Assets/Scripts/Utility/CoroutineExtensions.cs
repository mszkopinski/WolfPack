using System;
using System.Collections;

namespace Wolfpack
{
    public static class CoroutineExtensions
    {
        public static void Run(this IEnumerator enumerator)
        {
            CoroutineRunner.Instance.Run(enumerator);
        }

        public static void RunWithDelay(this IEnumerator enumerator, float delayInS)
        {
            CoroutineRunner.Instance.Run(enumerator, delayInS);
        }
        
        public static void Run(this IEnumerator enumerator, Action callback)
        {
            CoroutineRunner.Instance.Run(enumerator, callback);
        }
        
        public static void RunWithDelay(this IEnumerator enumerator, float delayInS, Action callback)
        {
            CoroutineRunner.Instance.Run(enumerator, delayInS, callback);
        }
    }
}