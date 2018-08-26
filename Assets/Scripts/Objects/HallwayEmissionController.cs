using System.Linq;
using UnityEngine;

namespace Wolfpack
{
    public class HallwayEmissionController : MonoBehaviour
    {
        Material[] materials;

        [SerializeField] Color glowColor;
        [SerializeField, Range(1f, 5f)] float emissionPower;
        [SerializeField] AudioClip glowSound;

        AudioSource audioSource;
        bool isEnabled;
    
        void Awake()
        {
            audioSource = GetComponentInParent<AudioSource>();
            materials = GetComponentsInChildren<Renderer>()?
                .SelectMany(renderer => renderer.materials)
                .Where(material => material.name.ToLower().Contains("glow")).ToArray();

            materials.SetColorProperty("_EmissionColor", new Color(0, 0, 0, 0));
        }
    
        void OnTriggerEnter(Collider collider)
        {
            if (collider.IsWolf() && !isEnabled)
            {
                isEnabled = true;
                audioSource.PlayOneShot(glowSound);
                materials.SetColorProperty("_EmissionColor", glowColor * Mathf.LinearToGammaSpace (emissionPower));
            }
        }
    
        void OnTriggerExit(Collider collider)
        {
            if (collider.IsWolf() && isEnabled)
            {
                isEnabled = false;
                materials.SetColorProperty("_EmissionColor", new Color(0, 0, 0, 0));
            }
        }
    }
}