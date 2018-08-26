using UnityEngine;

namespace Wolfpack
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(Animator))]
    public class HeadCameraRotationController : MonoBehaviour
    {
        [Header("Rotation Settings")] 
        [SerializeField] 
        float verticalMouseSensitivity = 2f;
        [SerializeField] 
        float horizontalMouseSensitivity = 1.2f;
        [SerializeField] 
        bool isVerticallyInversed;
        [SerializeField] 
        bool isHorizontallyInversed;
        [SerializeField] 
        float maxVerticalLookRotation = 80f;
        [SerializeField] 
        float maxHorizontalLookRotation = 80f;
    
        PlayerInput playerInput;
        Camera headCam;
        Animator animator;

        void Awake()
        {
            headCam = Camera.main;
            playerInput = GetComponent<PlayerInput>();
            animator = GetComponent<Animator>();
        
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            playerInput?.OnUpdate();
        
            RotateHead(playerInput.MouseX, playerInput.MouseY);
        }

        void RotateHead(float mouseX, float mouseY)
        {
            var rotationX = mouseY * verticalMouseSensitivity * Time.deltaTime;
            var rotationY = mouseX * horizontalMouseSensitivity * Time.deltaTime;
            var targetHeadRotation = headCam.transform.rotation.eulerAngles;

            var verticalRotation = isVerticallyInversed ? targetHeadRotation.x - rotationX : targetHeadRotation.x + rotationX;
            var horizontalRotation = isHorizontallyInversed ? targetHeadRotation.y - rotationY : targetHeadRotation.y + rotationY;
        
            targetHeadRotation.x = verticalRotation;
            targetHeadRotation.y = horizontalRotation;
            targetHeadRotation.z = 0f;
        
            // clamp vertical rotation
            if (verticalRotation >= maxVerticalLookRotation && verticalRotation <= maxVerticalLookRotation + 30f)
            {
                targetHeadRotation.x = maxVerticalLookRotation;
            }
            else if (verticalRotation <= 360f - maxVerticalLookRotation &&
                     verticalRotation >= 360f - maxVerticalLookRotation - 30f)
            {
                targetHeadRotation.x = 360f - maxVerticalLookRotation;
            }
        
            // clamp horizontal rotation
            if (horizontalRotation >= 90f + maxHorizontalLookRotation && horizontalRotation <= 90f + maxHorizontalLookRotation + 30f)
            {
                targetHeadRotation.y = 90f + maxHorizontalLookRotation;
            }
            else if (horizontalRotation <= 90f - maxHorizontalLookRotation &&
                     horizontalRotation >= 90f - maxHorizontalLookRotation - 30f)
            {
                targetHeadRotation.y = 90f - maxHorizontalLookRotation;
            }
        
            headCam.transform.rotation = Quaternion.Euler(targetHeadRotation);
        }
    }
}
