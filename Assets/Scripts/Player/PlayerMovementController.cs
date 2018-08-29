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
        Camera headCamera;
        Line currentMovementLine;
        float currentVelocity;
        float defaultFieldOfView;
        float currentStamina;

        void Awake()
        {
            animator = GetComponent<Animator>();
            wolf = GetComponent<Wolf>();
            headCamera = GetComponentInChildren<Camera>();
        }

        void Start()
        {
            input = GameManager.Instance.Input;    
            animator.SetFloat("vertical", 1f);
            defaultFieldOfView = headCamera.fieldOfView;
            animator.speed = Settings.DefaultAnimatorSpeed;
            currentVelocity = Settings.DefaultMovementVelocity;
            currentStamina = Settings.DefaultStamina;
            currentMovementLine = Settings.DefaultMovementLine;
            transform.ChangePosition("z", MovementHelper.Lines[currentMovementLine]);
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
            
            if (input.OnHorizontalDown && currentStamina >= Settings.JumpStaminaCost)
            {
                StartCoroutine(wolf.GlitchEffect.PlayGlitchEffectOnce(() =>
                {
                    currentStamina -= Settings.JumpStaminaCost;
                    ChangeLine(input.Horizontal);
                }));
            }
        }   

        void HandleAccelerationEffects()
        {
            var velocityMultiplier = -(1f - currentVelocity / Settings.DefaultMovementVelocity); // Range(0f, 1f)

            // sound effects
            GameManager.Instance.AudioMixer.SetVolume("musicVolume", 1f - velocityMultiplier * 0.15f);
            GameManager.Instance.AudioMixer.SetVolume("noiseVolume", velocityMultiplier * 0.8f);
            GameManager.Instance.AudioMixer.SetVolume("sfxVolume", 1f - velocityMultiplier * 0.4f);

            // other effects
            currentStamina = Mathf.MoveTowards(currentStamina,
                input.Vertical > 0f ? 0f : 100f,
                (input.Vertical > 0f ? 15f * velocityMultiplier : 15f) * Time.deltaTime);
            headCamera.SetFieldOfView(defaultFieldOfView + velocityMultiplier * 30f);
            animator.SetPlaybackSpeed(Settings.DefaultAnimatorSpeed + velocityMultiplier * Settings.AnimatorSpeedMultiplier);
        }
        
        void ChangeLine(float horizontalInput)
        {
            switch (currentMovementLine)
            {
                case Line.Left:
                    currentMovementLine = horizontalInput > 0f ? Line.Center : Line.Left;
                    break;
                case Line.Center:
                    currentMovementLine = horizontalInput > 0f ? Line.Right : Line.Left;
                    break;
                case Line.Right:
                    currentMovementLine = horizontalInput > 0f ? Line.Right : Line.Center;
                    break;
            }

            transform.ChangePosition("z", MovementHelper.Lines[currentMovementLine]);
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0, 75, 250, 25), $"Player Velocity: {currentVelocity}");
            GUI.Label(new Rect(0, 100, 250, 25), $"Player Energy: {currentStamina}");
        }
    }
}
