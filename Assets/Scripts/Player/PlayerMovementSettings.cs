using UnityEngine;
using UnityEngine.Audio;

namespace Wolfpack
{        
    [CreateAssetMenu(fileName = "PlayerMovementSettings", menuName = "WolfPack/Player Movement Settings")]
    public class PlayerMovementSettings : ScriptableObject
    {
        [Header("Movement")] 
        public float MaxMovementVelocity = 20f;
        public float Acceleration = 2f;
        public float DefaultMovementVelocity = 10f;

        [Header("Stamina")] 
        public float MaxStamina = 100f;
        public float DefaultStamina = 100f;
        public float JumpStaminaCost = 25f;
        
        [Header("Animation")]
        public float DefaultAnimatorSpeed = 1f;
        public float AnimatorSpeedMultiplier = 1f;
    }
}