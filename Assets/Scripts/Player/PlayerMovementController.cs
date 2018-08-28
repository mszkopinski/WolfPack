using UnityEngine;
using UnityEngine.Audio;
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
        }
    
        void Update()
        {
            HandleAccelerationEffects();            
            
            if (input == null)
                return;

            input.OnUpdate();

            if (input.OnHorizontalDown)
            {
                var horizontal = input.Horizontal;
                Settings.AudioMixer.SetFloat("sfxVol", 15f);
                Settings.AudioMixer.SetFloat("musicVol", -15f);
                StartCoroutine(wolf.GlitchEffect.PlayGlitchEffectOnce(() => TeleportWolf(horizontal)));
            }

            currentVelocity = Mathf.MoveTowards(currentVelocity, 
                input.Vertical > 0f 
                    ? Settings.MaxMovementVelocity
                    : Settings.DefaultMovementVelocity,
                (input.Vertical > 0f
                    ? Settings.Acceleration
                    : Settings.Acceleration * 0.6f) * Time.deltaTime);

            transform.position += transform.forward * currentVelocity * Time.deltaTime;
        }

        void HandleAccelerationEffects()
        {
            var velocityMultiplier = -(1f - currentVelocity / Settings.DefaultMovementVelocity);
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
        }
    }
}
