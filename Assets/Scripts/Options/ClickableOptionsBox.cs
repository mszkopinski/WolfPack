using System.Linq;
using UnityEngine;
using Zenject;

namespace Wolfpack
{
    public class ClickableOptionsBox : MonoBehaviour
    {
        public GameLifecycleAction Action;

        Material[] materials;
        AudioSource audioSource;
        Vector4 defaultTiling;
        float defaultHologramVelocity;
        float defaultGlowIntensity;
        bool IsMenuLoaded => GameManager.Instance.State.Status == GameStatus.Menu;

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            materials = GetComponentsInChildren<Renderer>()?.SelectMany(renderer => renderer.materials).Where(material => material.name.Contains("box_mat")).ToArray();

            var mat = materials[0];
            defaultTiling = mat.GetVector("_Hologram_Texture_Tiling");
            defaultHologramVelocity = mat.GetFloat("_Hologram_Velocity");
            defaultGlowIntensity = mat.GetFloat("_Glow_Intensity");
        }

        void OnMouseDown()
        {
            if (!IsMenuLoaded) return;
        
            switch (Action)
            {
                case GameLifecycleAction.StartGame:
                    GameManager.Instance.LoadGame();
                    break;
                case GameLifecycleAction.ExitGame:
                    GameManager.Instance.Level.Quit();
                    break;
            }
        }

        void OnMouseEnter()
        {
            if (!IsMenuLoaded) return;
            
            audioSource.Play();
//            GameManager.Instance.AudioMixer.SetFloat("musicPitch", 0.98f);
            var mat = materials[0];
            mat.SetVector("_Hologram_Texture_Tiling", new Vector4(0f, 20f, 0f, 0f));
            mat.SetFloat("_Hologram_Velocity", -1f);
            mat.SetFloat("_Glow_Intensity", .1f);
        }

        void OnMouseExit()
        {
            if (!IsMenuLoaded) return;
            
            audioSource.Stop();
//            GameManager.Instance.AudioMixer.SetFloat("musicPitch", 1f);
            var mat = materials[0];
            mat.SetVector("_Hologram_Texture_Tiling", defaultTiling);
            mat.SetFloat("_Hologram_Velocity", defaultHologramVelocity);
            mat.SetFloat("_Glow_Intensity", defaultGlowIntensity);
        }
    }
}

