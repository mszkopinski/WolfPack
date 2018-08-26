using UnityEngine;

namespace Wolfpack
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Settings")] 
        [SerializeField] float movementVelocity = 20f;
    
        PlayerInput playerInput;
        Animator animator;

        void Awake()
        {
            animator = GetComponent<Animator>();
            playerInput = GetComponent<PlayerInput>();
        }
    
        void Update()
        {
            playerInput?.OnUpdate();
            animator.SetFloat("vertical", playerInput.VerticalAxis);
            transform.position += playerInput.VerticalAxis * transform.forward * movementVelocity * Time.deltaTime;
        }
    }
}
