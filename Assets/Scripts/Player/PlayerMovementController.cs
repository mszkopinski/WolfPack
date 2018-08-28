using UnityEngine;
using WolfPack;

namespace Wolfpack
{
    [RequireComponent(typeof(Wolf))]
    public class PlayerMovementController : MonoBehaviour
    {
        public PlayerMovementSettings Settings;

        IInputController input;
        Wolf wolf;
        Animator animator;
        Camera camera;
        float currentVelocity;
        float defaultFieldOfView;
        float currentStamina;

        void Awake()
        {
            animator = GetComponent<Animator>();
            wolf = GetComponent<Wolf>();
            camera = GetComponentInChildren<Camera>();
        }

        void Start()
        {
            input = GameManager.Instance.Input;    
            animator.SetFloat("vertical", 1f);
            animator.speed = Settings.DefaultAnimatorSpeed;
            currentVelocity = Settings.DefaultMovementVelocity;
            defaultFieldOfView = camera.fieldOfView;
            currentStamina = 100f;
        }
    
        void Update()
        {
            HandleAccelerationEffects();            
            
            if (input == null)
                return;

            input.OnUpdate();
            var isForwardPressed = input.Vertical > 0f;
            
            currentVelocity = Mathf.MoveTowards(currentVelocity, 
                isForwardPressed
                    ? Settings.MaxMovementVelocity
                    : Settings.DefaultMovementVelocity,
                (isForwardPressed
                    ? Settings.Acceleration
                    : Settings.Acceleration * 0.6f) * Time.deltaTime);

            transform.position += transform.forward * currentVelocity * Time.deltaTime;
            
            if (input.OnHorizontalDown)
            {
                var horizontal = input.Horizontal;
                Settings.AudioMixer.SetFloat("sfxVol", 15f);
                Settings.AudioMixer.SetFloat("musicVol", -15f);
                StartCoroutine(wolf.GlitchEffect.PlayGlitchEffectOnce(() => TeleportWolf(horizontal)));
            }
        }

        void HandleAccelerationEffects()
        {
            var velocityMultiplier = -(1f - currentVelocity / Settings.DefaultMovementVelocity); // Range(0f, 1f)
            currentStamina = Mathf.MoveTowards(currentStamina,
                input.Vertical > 0f ? 0f : 100f,
                (input.Vertical > 0f ? 15f * velocityMultiplier : 15f) * Time.deltaTime);
            camera.SetFieldOfView(defaultFieldOfView + velocityMultiplier * 30f);
            animator.SetPlaybackSpeed(Settings.DefaultAnimatorSpeed + velocityMultiplier * Settings.AnimatorSpeedMultiplier);
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
            GUI.Label(new Rect(0, 100, 250, 25), $"Player Energy: {currentStamina}");
        }
    }
}
