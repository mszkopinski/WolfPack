using UnityEngine;

namespace Wolfpack
{
    [RequireComponent(typeof(Wolf))]
    public class WolfAutoMovementController : MonoBehaviour
    {
        [Header("Movement Settings")] 
        [SerializeField] float movementVelocity = 20f;
        [SerializeField] float teleportDistance = 2.3f;

        public Line CurrentMovementLine;
        
        Wolf wolf;
        Animator animator;
        bool canMove = true;

        void Awake()
        {
            animator = GetComponent<Animator>();
            wolf = GetComponent<Wolf>();
        }
    
        void Start()
        {
            GameManager.Instance.State.StatusChanged += OnStatusChanged;
            wolf.GlitchEffect.OnGlitch += TeleportWolf;
            movementVelocity = 16f;
            animator.speed = 1f + 1f / 12f * movementVelocity;
        }

        void Update()
        {
            if (!canMove)
                return;

            animator.SetFloat("vertical", 1f);
            transform.position += transform.forward * movementVelocity * Time.deltaTime;
        }

        void TeleportWolf()
        {
            CurrentMovementLine = GetRandomTeleportLine(CurrentMovementLine);
            transform.ChangePosition("x", MovementHelper.LinePositions[CurrentMovementLine]);
        }

        Line GetRandomTeleportLine(Line currentMovementLine)
        {
            var newLine = currentMovementLine;
            while (newLine == currentMovementLine)
                newLine = MovementHelper.GetRandomLine();
            return newLine;
        }

        void OnStatusChanged()
        {
            var status = GameManager.Instance.State.Status;
            Log.Console(status);

            if (status == GameStatus.Game)
                canMove = true;
        }
    }
}