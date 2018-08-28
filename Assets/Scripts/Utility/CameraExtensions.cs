using UnityEngine;

namespace WolfPack
{
    public static class CameraExtensions
    {
        public static void SetFieldOfView(this Camera camera, float value)
        {
            camera.fieldOfView = value;
        }
    }
}