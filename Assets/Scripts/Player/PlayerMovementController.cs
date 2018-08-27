using UnityEngine;
using Zenject;

namespace Wolfpack
{
    public class PlayerMovementController : MonoBehaviour
    {
        [Header("Movement Settings")] 
        [SerializeField] float movementVelocity = 20f;
              
        IControllerInput input;
        Animator animator;

        [Inject]
        public void Initialize(IControllerInput controllerInput) { input = controllerInput; }

        void Awake()
        {
            animator = GetComponent<Animator>();
        }
    
        void Update()
        {
            if (input == null)
                return;
            
            input.OnUpdate();
            animator.SetFloat("vertical", input.VerticalAxis);
            transform.position += input.VerticalAxis * transform.forward * movementVelocity * Time.deltaTime;
        }
    }
}
