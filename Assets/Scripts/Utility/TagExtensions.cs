using UnityEngine;

namespace Wolfpack
{
    public static class TagExtensions
    {
        public static bool IsWolf(this Collider col)
        {
            return col.gameObject.CompareTag("Wolf");
        }
        
        public static bool IsPlayer(this Collider col)
        {
            return col.gameObject.CompareTag("Player");
        }
    }
}