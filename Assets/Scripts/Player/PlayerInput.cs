using UnityEngine;

namespace Wolfpack
{
    public class PlayerInput : MonoBehaviour
    {
        public float HorizontalAxis { get; private set; }
        public float VerticalAxis { get; private set; }
    
        public float MouseX { get; private set; }
        public float MouseY { get; private set; }
    
        public void OnUpdate()
        {
            HorizontalAxis = Input.GetAxis("Horizontal");
            VerticalAxis = Input.GetAxis("Vertical");
            MouseX = Input.GetAxis("Mouse X");
            MouseY = Input.GetAxis("Mouse Y");
        }
    }
}