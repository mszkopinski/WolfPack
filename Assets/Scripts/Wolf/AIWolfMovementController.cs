using System.Collections;
using UnityEngine;

namespace Wolfpack
{
    [RequireComponent(typeof(Wolf))]
    public class AIWolfMovementController : MonoBehaviour
    {
        [Header("Movement Settings")] 
        [SerializeField] float startVelocity = 16f;

        [Header("Obstacle avoidance"), Range(0f, 100f)]
        public float ObstacleAvoidanceChances = 80f;

        public Line CurrentLine { get; set; }
        public bool CanRandomlyTeleport { get; set; }
        
        Wolf wolf;
        Animator animator;
        float currentVelocity;
        bool canMove = true;

        void Awake()
        {
            animator = GetComponent<Animator>();
            wolf = GetComponent<Wolf>();
        }
    
        void Start()
        {
            GameManager.Instance.State.StatusChanged += OnStatusChanged;
            wolf.GlitchEffect.OnGlitch += () =>
            {
                if (!CanRandomlyTeleport)
                    return;
                
                CurrentLine = MovementHelper.GetRandomLine(CurrentLine);
                TeleportWolf(CurrentLine);
            };
            currentVelocity = startVelocity;
            animator.speed = 1f + 1f / 12f * currentVelocity;
        }

        void Update()
        {
            if (!canMove)
                return;

            animator.SetFloat("vertical", 1f);
            transform.position += transform.forward * currentVelocity * Time.deltaTime;
        }

        public void TeleportWolf(Line line)
        {
            transform.ChangePosition("x", MovementHelper.LinePositions[line]);
        }
        
        public IEnumerator RestoreAbilityToTeleport()
        {
            yield return null;
            CanRandomlyTeleport = true;
        }

        void OnStatusChanged()
        {
            if (GameManager.Instance.State.Status == GameStatus.Game)
                canMove = true;
        }
    }
}