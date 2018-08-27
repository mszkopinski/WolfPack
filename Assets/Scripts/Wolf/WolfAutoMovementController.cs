using UnityEngine;
using Zenject;

namespace Wolfpack
{
    [RequireComponent(typeof(GlitchEffectController))]
    public class WolfAutoMovementController : MonoBehaviour
    {
        [Header("Movement Settings")] 
        [SerializeField] float movementVelocity = 20f;
        [SerializeField] float teleportDistance = 2.3f;

        GlitchEffectController glitchEffectController;
        Animator animator;
        bool canMove = true;

        void Awake()
        {
            animator = GetComponent<Animator>();
            glitchEffectController = GetComponent<GlitchEffectController>();
        }
    
        void Start()
        {
            GameManager.Instance.State.StatusChanged += OnStatusChanged;
            glitchEffectController.GlitchEffectPlayed += TeleportWolf;
            
            movementVelocity = Random.Range(12f, 16f);
            glitchEffectController.DefaultGlowIntensity = 3f - 1f / 6f * movementVelocity;
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