using UnityEngine;

namespace Wolfpack
{
    public class PlayerInput : IControllerInput
    {
        public float HorizontalAxis { get; private set; }
        public float VerticalAxis { get; private set; }
        public float MouseHorizontalAxis { get; private set; }
        public float MouseVerticalAxis { get; private set; }

        public void OnUpdate()
        {
            HorizontalAxis = Input.GetAxis("Horizontal");
            VerticalAxis = Input.GetAxis("Vertical");
            MouseHorizontalAxis = Input.GetAxis("Mouse X");
            MouseVerticalAxis = Input.GetAxis("Mouse Y");
        }
    }
}