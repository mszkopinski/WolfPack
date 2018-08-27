using UnityEngine;

namespace Wolfpack
{
    public class InputController : IInputController
    {
        public bool OnVerticalDown { get; private set; }
        public bool OnHorizontalDown { get; private set; }
        public float Horizontal { get; private set; }
        public float Vertical { get; private set; }
        public float MouseX { get; private set; }
        public float MouseY { get; private set; }

        public void OnUpdate()
        {
            OnVerticalDown = Input.GetButtonDown("Vertical");
            OnHorizontalDown = Input.GetButtonDown("Horizontal");
            Horizontal = Input.GetAxis("Horizontal");
            Vertical = Input.GetAxis("Vertical");
            MouseX = Input.GetAxis("Mouse X");
            MouseY = Input.GetAxis("Mouse Y");
        }
    }
}