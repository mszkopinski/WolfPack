using UnityEngine;

namespace Wolfpack
{
    [RequireComponent(typeof(Wolf))]
    public class WolfAutoMovementController : MonoBehaviour
    {
        [Header("Movement Settings")] 
        [SerializeField] float movementVelocity = 20f;
        [SerializeField] float teleportDistance = 2.3f;

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
            movementVelocity = Random.Range(12f, 16f);
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
            var newPosition = transform.position;
            newPosition.z = GetRandomTeleportDistance(transform.position.z);
            transform.position = newPosition;
        }

        float GetRandomTeleportDistance(float currentZ)
        {
            float distance; 
            var randomNumber = Random.Range(0, 10);
            if (randomNumber <= 2)
                distance = teleportDistance;
            else if (randomNumber > 2 && randomNumber <= 5f)
                distance = -teleportDistance;
            else
                distance = 0f;

            return Mathf.RoundToInt(distance) == Mathf.RoundToInt(currentZ)
                ? GetRandomTeleportDistance(currentZ)
                : distance;
        }

        void OnStatusChanged()
        {
            var status = GameManager.Instance.State.Status;
            Debug.Log(status);

            if (status == GameStatus.Game)
                canMove = true;
        }
    }
}