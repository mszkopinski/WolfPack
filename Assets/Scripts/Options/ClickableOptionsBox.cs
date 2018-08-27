using System.Linq;
using UnityEngine;

namespace Wolfpack
{
    public class ClickableOptionsBox : MonoBehaviour
    {
        #region Dependency Injection 

        readonly IGameState gameState;
        readonly ILevelManager levelManager;

        public ClickableOptionsBox(IGameState gameState, ILevelManager levelManager)
        {
            this.gameState = gameState;
            this.levelManager = levelManager;
        }

        #endregion

        public GameLifecycleAction Action;

        Material[] materials;
        AudioSource audioSource;
        Vector4 defaultTiling;
        float defaultHologramVelocity;
        float defaultGlowIntensity;
        bool IsMenuLoaded => gameState.Value.Status == GameStatus.Menu;

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
                    FadeInImage.Instance.Fade(FadeDirection.In, 1f);
                    StartCoroutine(levelManager.LoadLevelWithDelay(LevelName.Game.ToString(), 3f));
                    break;
                case GameLifecycleAction.ExitGame:
                    levelManager.Quit();
                    break;
            }
        }

        void OnMouseEnter()
        {
            if (!IsMenuLoaded) return;
            
            audioSource.Play();
            var mat = materials[0];
            mat.SetVector("_Hologram_Texture_Tiling", new Vector4(0f, 20f, 0f, 0f));
            mat.SetFloat("_Hologram_Velocity", -1f);
            mat.SetFloat("_Glow_Intensity", .1f);
        }

        void OnMouseExit()
        {
            if (!IsMenuLoaded) return;
            
            audioSource.Stop();
            var mat = materials[0];
            mat.SetVector("_Hologram_Texture_Tiling", defaultTiling);
            mat.SetFloat("_Hologram_Velocity", defaultHologramVelocity);
            mat.SetFloat("_Glow_Intensity", defaultGlowIntensity);
        }
    }
}

