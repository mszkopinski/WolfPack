using UnityEngine;

namespace Wolfpack
{
    public static class MaterialExtensions
    {
        public static void SetColorProperty(this Material[] materials, string propertyName, Color value)
        {
            foreach (var material in materials)
                material.SetColor(propertyName, value);
        }
    
        public static void SetFloatProperty(this Material[] materials, string propertyName, float value)
        {
            foreach (var material in materials)
                material.SetFloat(propertyName, value);
        }
    
        public static void SetVector4Property(this Material[] materials, string propertyName, Vector4 value)
        {
            foreach (var material in materials)
                material.SetVector(propertyName, value);
        }
    }
}
