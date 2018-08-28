using System;
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
            currentStamina = Settings.DefaultStamina;
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
                StartCoroutine(wolf.GlitchEffect.PlayGlitchEffectOnce(() =>
                {
                    currentStamina -= Settings.JumpStaminaCost;
                    ChangeLine(horizontal);
                }));
            }

            if (input.OnVerticalDown)
            {

            }
        }   

        void HandleAccelerationEffects()
        {
            var velocityMultiplier = -(1f - currentVelocity / Settings.DefaultMovementVelocity); // Range(0f, 1f)

//            GameManager.Instance.AudioMixer.SetFloat("musicPitch", 0.9f + 0.1f * velocityMultiplier);
//            GameManager.Instance.AudioMixer.SetFloat("musicPitch", 1f - 0.1f * velocityMultiplier);
            
            GameManager.Instance.AudioMixer.SetVolume("musicVolume", 1f - velocityMultiplier * 0.15f);
            GameManager.Instance.AudioMixer.SetVolume("noiseVolume", velocityMultiplier * 0.8f);
            GameManager.Instance.AudioMixer.SetVolume("sfxVolume", 1f - velocityMultiplier * 0.4f);
            currentStamina = Mathf.MoveTowards(currentStamina,
                input.Vertical > 0f ? 0f : 100f,
                (input.Vertical > 0f ? 15f * velocityMultiplier : 15f) * Time.deltaTime);
            camera.SetFieldOfView(defaultFieldOfView + velocityMultiplier * 30f);
            animator.SetPlaybackSpeed(Settings.DefaultAnimatorSpeed + velocityMultiplier * Settings.AnimatorSpeedMultiplier);
        }
        
        void ChangeLine(float horizontalInput)
        {
            var tolerance = 0.2f;
            var shouldGoRight = horizontalInput > 0f;
            var newPosition = transform.position;

            if (shouldGoRight)
            {
                if (Math.Abs(newPosition.z - Paths.Center) < tolerance)
                    newPosition.z = Paths.RightMost;
                else if (Math.Abs(newPosition.z - Paths.LeftMost) < tolerance)
                    newPosition.z = Paths.Center;
            }
            else
            {
                if (Math.Abs(newPosition.z - Paths.RightMost) < tolerance)
                    newPosition.z = Paths.Center;
                else if (Math.Abs(newPosition.z - Paths.Center) < tolerance)
                    newPosition.z = Paths.LeftMost; 
            }

            transform.position = newPosition;
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0, 75, 250, 25), $"Player Velocity: {currentVelocity}");
            GUI.Label(new Rect(0, 100, 250, 25), $"Player Energy: {currentStamina}");
        }
    }
}
