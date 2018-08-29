using UnityEngine;
using Zenject;

namespace Wolfpack
{
    [RequireComponent(typeof(Animator))]
    public class CameraRotationController : MonoBehaviour
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
    
        Camera _camera;
        IInputController input;

        void Awake()
        {
            _camera = Camera.main;
            input = GameManager.Instance.Input;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        void Update()
        {
            if (input == null)
                return;
            
            input.OnUpdate();
            RotateHead(input.MouseX, input.MouseY);
        }

        void RotateHead(float mouseX, float mouseY)
        {
            var rotationX = mouseY * verticalMouseSensitivity * Time.deltaTime;
            var rotationY = mouseX * horizontalMouseSensitivity * Time.deltaTime;
            var targetHeadRotation = _camera.transform.rotation.eulerAngles;

            var verticalRotation = isVerticallyInversed ? targetHeadRotation.x - rotationX : targetHeadRotation.x + rotationX;
            var horizontalRotation = isHorizontallyInversed ? targetHeadRotation.y - rotationY : targetHeadRotation.y + rotationY;
        
            targetHeadRotation.x = verticalRotation;
            targetHeadRotation.y = horizontalRotation;
            targetHeadRotation.z = 0f;
        
            if (verticalRotation >= maxVerticalLookRotation && verticalRotation <= maxVerticalLookRotation + 30f)
            {
                targetHeadRotation.x = maxVerticalLookRotation;
            }
            else if (verticalRotation <= 360f - maxVerticalLookRotation &&
                     verticalRotation >= 360f - maxVerticalLookRotation - 30f)
            {
                targetHeadRotation.x = 360f - maxVerticalLookRotation;
            }
            
            if (horizontalRotation >= maxHorizontalLookRotation && horizontalRotation <= maxHorizontalLookRotation + 30f)
            {
                targetHeadRotation.y = maxHorizontalLookRotation;
            }
            else if (horizontalRotation <= 360f - maxHorizontalLookRotation &&
                     horizontalRotation >= 360f - maxHorizontalLookRotation - 30f)
            {
                targetHeadRotation.y = 360f - maxHorizontalLookRotation;
            }
        
            _camera.transform.rotation = Quaternion.Euler(targetHeadRotation);
        }
    }
}
