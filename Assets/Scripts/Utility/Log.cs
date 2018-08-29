using UnityEngine;

namespace Wolfpack
{
    public static class Log
    {
        public static void Console(object message)
        {
#if UNITY_EDITOR
            Debug.Log(message);
#endif
        }
    }
}