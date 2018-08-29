using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;
using WolfPack;

namespace Wolfpack
{
    [RequireComponent(typeof(Wolf))]
    public class PlayerMovementController : MonoBehaviour
    {
        public PlayerMovementSettings Settings;

        [SerializeField] Canvas staminaCanvas;
        [SerializeField] Canvas cooldownCanvas;
        [SerializeField] TextMeshProUGUI velocityText;
        [SerializeField] TextMeshProUGUI staminaText;
        [SerializeField] AudioClip noStaminaAudio;

        IInputController input;
        Wolf wolf;
        Animator animator;
        Camera headCamera;
        Line currentMovementLine;
        AudioSource noStaminaAudioSource;
        float currentVelocity;
        float defaultFieldOfView;
        float currentStamina;
        bool canMove = true;

        void Awake()
        {
            animator = GetComponent<Animator>();
            wolf = GetComponent<Wolf>();
            headCamera = GetComponentInChildren<Camera>();
            noStaminaAudioSource = GetComponents<AudioSource>().GetAudioSourceWithPriority(256);
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
            transform.ChangePosition("z", MovementHelper.LinePositions[currentMovementLine]);
            cooldownCanvas.gameObject.SetActive(false);
            staminaCanvas.enabled = false;
        }
    
        void Update()
        {
            HandleAccelerationEffects();            
            
            if (input == null)
                return;

            input.OnUpdate();
            
            var isForwardPressed = input.Vertical > 0f;
            
            currentVelocity = Mathf.MoveTowards(currentVelocity, 
                canMove && isForwardPressed && currentStamina > 0f 
                    ? Settings.MaxMovementVelocity
                    : Settings.DefaultMovementVelocity,
                (canMove && isForwardPressed && currentStamina > 0f
                    ? Settings.Acceleration
                    : canMove ? Settings.Acceleration * 0.6f : Settings.Acceleration * 2f) * Time.deltaTime);

            transform.position += transform.forward * currentVelocity * Time.deltaTime;
            
            if (canMove && input.OnHorizontalDown && currentStamina >= Settings.JumpStaminaCost)
            {
                ChangeLine(input.Horizontal);
                StartCoroutine(wolf.GlitchEffect.PlayGlitchEffectOnce(() =>
                {
                    currentStamina -= Settings.JumpStaminaCost;
                }));
            }

            velocityText.text = (Math.Round(currentVelocity, 2) * 2.3).ToString(CultureInfo.InvariantCulture) + " bytes";
            staminaText.text = Mathf.RoundToInt(currentStamina).ToString(CultureInfo.InvariantCulture);
            staminaCanvas.enabled = currentStamina <= 0f;
            
            if (currentStamina <= 0f && !noStaminaAudioSource.isPlaying)
            {
                staminaText.enabled = false;
                noStaminaAudioSource.PlayOneShot(noStaminaAudio);
                AccelerationCooldown().RunWithDelay(noStaminaAudio.length - 0.1f);
            }
        }

        IEnumerator AccelerationCooldown()
        {
            GameManager.Instance.AudioMixer.ChangeFloatOverTime("musicPitch", 0.4f, Settings.StaminaCooldown).Run();
            canMove = false;
            staminaCanvas.enabled = false;
            cooldownCanvas.gameObject.SetActive(true);
            yield return new WaitForSeconds(Settings.StaminaCooldown);
            GameManager.Instance.AudioMixer.ChangeFloatOverTime("musicPitch", 1f, 1f).Run();
            cooldownCanvas.gameObject.SetActive(false);
            canMove = true;
            staminaText.enabled = true;
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
                canMove && input.Vertical > 0f ? 0f : 100f,
                (canMove && input.Vertical > 0f ? 15f * velocityMultiplier : 15f) * Time.deltaTime);
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

            transform.ChangePosition("x", MovementHelper.LinePositions[currentMovementLine]);
        }

        void OnGUI()
        {
            GUI.Label(new Rect(0, 75, 250, 25), $"Player Velocity: {currentVelocity}");
            GUI.Label(new Rect(0, 100, 250, 25), $"Player Energy: {currentStamina}");
        }
    }
}
