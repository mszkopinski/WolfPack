using UnityEngine;

namespace Wolfpack
{
    [RequireComponent(typeof(Wolf))]
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] 
        Camera headCamera;
        
        [Header("Movement Settings")] 
        [SerializeField]
        float maxMovementVelocity = 20f;
        [SerializeField] 
        float acceleration = 2f;
        [SerializeField] 
        float defaultMovementVelocity = 10f;
        [SerializeField]
        float defaultAnimatorSpeed = 1f;
        [SerializeField]
        float animatorSpeedMultiplier = 1f;

        Wolf wolf;
        IInputController input;
        Animator animator;
        
        float currentVelocity;
        float defaultCameraFOV;

        void Awake()
        {
            animator = GetComponent<Animator>();
            wolf = GetComponent<Wolf>();
        }

        void Start()
        {
            input = GameManager.Instance.Input;
            currentVelocity = defaultMovementVelocity;
            animator.SetFloat("vertical", 1f);
            animator.speed = defaultAnimatorSpeed;
            defaultCameraFOV = headCamera.fieldOfView;
        }
    
        void Update()
        {
            if (input == null)
                return;
            
            input.OnUpdate();

            if (input.OnHorizontalDown)
            {
                var horizontal = input.Horizontal;
                StartCoroutine(wolf.GlitchEffect.PlayGlitchEffectOnce(() => TeleportWolf(horizontal)));
            }

            currentVelocity = Mathf.MoveTowards(currentVelocity, 
                input.Vertical > 0f 
                    ? maxMovementVelocity
                    : defaultMovementVelocity,
                (input.Vertical > 0f
                    ? acceleration
                    : acceleration * 0.6f) * Time.deltaTime);

            var velocityMultiplier = -(1f - currentVelocity / defaultMovementVelocity);
            headCamera.fieldOfView = defaultCameraFOV + velocityMultiplier * 20f;
            animator.speed = defaultAnimatorSpeed + velocityMultiplier * animatorSpeedMultiplier;
            transform.position += transform.forward * currentVelocity * Time.deltaTime;
        }
        
        void TeleportWolf(float horizontalInput)
        {
            var newPosition = transform.position;
            newPosition.z = horizontalInput > 0f ? transform.position.z + 2.3f : transform.position.z - 2.3f;
            transform.position = newPosition;
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0, 75, 250, 25), $"Player Velocity: {currentVelocity}");
        }
    }
}
