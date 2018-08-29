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
    }
}