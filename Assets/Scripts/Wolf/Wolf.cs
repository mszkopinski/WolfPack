using System;
using System.Linq;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Wolfpack
{
    public sealed class Wolf : MonoBehaviour
    {
        public static Action<Wolf> Spawned;
        
        public IGlitchEffect GlitchEffect { get; private set; }
        public GlitchEffectSettings GlitchEffectSettings;

        AudioSource audioSource;

        [Inject]
        public void Init(IGlitchEffect glitchEffect)
        {
            GlitchEffect = glitchEffect;
        }

        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Start()
        {
            Spawned?.Invoke(this);
            
            GlitchEffect.Initialize(
                GetComponentsInChildren<Renderer>()
                    .SelectMany(renderer => renderer.materials)
                    .Where(material => material.name.ToLower().Contains("holo"))
                    .ToArray(),
                audioSource,
                GlitchEffectSettings
            );
            if (GlitchEffectSettings.IsGlitchAutomatic)
                StartCoroutine(GlitchEffect.PlayGlitchEffectConstantlyWithDelay());
            
            GameManager.Instance.Wolves.Add(this);
        }

        void OnDied()
        {
            var existingWolves = GameManager.Instance.Wolves;
            if (existingWolves.Contains(this)) existingWolves.Remove(this);
            ScreenFading.Instance.FadeInOut(FadeDirection.In, .1f).Run();
            Destroy(gameObject);
        }

        void OnTriggerEnter(Collider col)
        {
            if (!col.IsObstacle())
                return;
            
            var wolfAIMovement = GetComponent<AIWolfMovementController>();
            if (wolfAIMovement != null)
            {
                if (Random.Range(0f, 100f) < 100f - wolfAIMovement.ObstacleAvoidanceChances)
                {
                    (col.GetComponent(typeof(Obstacle)) as Obstacle)?.Destroy();
                    OnDied();
                    return;
                }
                wolfAIMovement.CanRandomlyTeleport = false;
                var nearestFormation = ObstacleFormationsSpawner.Instance.GetNearestFormation(transform);
                var possibleLines = MovementHelper.LinePositions
                    .Select(pair => pair.Key)
                    .Except(nearestFormation.OccupiedLines)
                    .ToList();
                var lineToTeleport = possibleLines.Count > 1 
                    ? possibleLines.FirstOrDefault(line => line != wolfAIMovement.CurrentLine) 
                    : possibleLines.First();
                GlitchEffect.PlayGlitchEffectOnce().Run();
                wolfAIMovement.TeleportWolf(lineToTeleport);
                wolfAIMovement.RestoreAbilityToTeleport().RunWithDelay(2f);
                return;
            }
            
            // if wolf is player
            if (col is SphereCollider)
            {
                (col.GetComponent(typeof(Obstacle)) as Obstacle)?.Destroy();
                OnDied();
            }
        }
    }
}