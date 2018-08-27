using System.Collections;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;
using Zenject;

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
            GameManager.Instance.State.StatusChanged += OnStatusChanged;
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

        void OnStatusChanged()
        {
            var status = GameManager.Instance.State.Status;
            
            if (status == GameStatus.Intro)
                StartCoroutine(PlayIntroAnimation());
        }

        IEnumerator PlayIntroAnimation()
        {
            animator.Play("Camera@Intro");
            yield return new WaitForSeconds(animator.GetClipLength("Camera@Intro"));
            GameManager.Instance.State.SetGameStatus(GameStatus.Menu);
        }
    }
}
