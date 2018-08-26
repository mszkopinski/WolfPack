using System.Collections;
using System.Linq;
using UnityEngine;

namespace Wolfpack
{
    public class CameraController : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] Vector3 positionOffset = Vector3.zero;

        [Header("Speed Settings")]
        [SerializeField] float smoothSpeed = 0.0001f;

        [Header("Triggers")]
        [SerializeField] bool lookAtTarget = true;
    
        Transform target;
        Animator animator;
        Vector3 velocity = Vector3.zero;
        Camera controlledCamera;

        void Awake()
        {
            controlledCamera = Camera.main;
            animator = GetComponent<Animator>();
        }

        void Start()
        {
            Wolf.WolfAppeared += SetTarget;
            GameManager.GameStateChanged += OnGameStateChanged;
        }

        void SetTarget(Transform target)
        {
            if (target != null)
                this.target = target;
        }

        void LateUpdate()
        {
            if (!target) return;
            var targetPos = target.position + positionOffset;
            controlledCamera.transform.position = Vector3.SmoothDamp(controlledCamera.transform.position, targetPos, ref velocity, smoothSpeed);
            if (lookAtTarget)
                controlledCamera.transform.LookAt(target);
        }

        void OnGameStateChanged(GameState state)
        {
            if (state == GameState.Intro)
                StartCoroutine(PlayIntroAnimation());
        }

        IEnumerator PlayIntroAnimation()
        {
            animator.Play("Camera@Intro");
            yield return new WaitForSeconds(
                animator.runtimeAnimatorController.animationClips.FirstOrDefault(clip => clip.name == "Camera@Intro").length);
            GameManager.Instance.SetGameState(GameState.Menu);
        }
    }
}
