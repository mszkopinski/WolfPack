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
        public static Action<Wolf> Died;
        
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
            Died?.Invoke(this);
            Destroy(gameObject, .2f);
        }

        void OnTriggerEnter(Collider col)
        {
            if (!col.IsObstacle())
                return;
            
            var wolfAIMovement = GetComponent<AIWolfMovementController>();
            if (wolfAIMovement != null)
            {
                var nearestFormation = ObstacleFormationsSpawner.Instance.GetNearestFormation(transform);

                if (Random.Range(0f, 100f) < 100f - wolfAIMovement.ObstacleAvoidanceChances
                    || nearestFormation == null)
                {
                    (col.GetComponent(typeof(Obstacle)) as Obstacle)?.Destroy();
                    OnDied();
                    return;
                }
                
                wolfAIMovement.CanRandomlyTeleport = false;
                var possibleLines = MovementHelper.LinePositions
                    .Select(pair => pair.Key)
                    .Except(nearestFormation.OccupiedLines)
                    .ToList();
                GlitchEffect.PlayGlitchEffectOnce().Run();
                wolfAIMovement.TeleportWolf(possibleLines.Count > 1 
                    ? possibleLines.FirstOrDefault(line => line != wolfAIMovement.CurrentLine) 
                    : possibleLines.First());
                wolfAIMovement.RestoreAbilityToTeleport().RunWithDelay(2f);
                return;
            }
            
            // if wolf is player
            if (col is SphereCollider)
            {
                (col.GetComponent(typeof(Obstacle)) as Obstacle)?.Destroy();
                (GetComponentInChildren(typeof(Camera)) as Camera).transform.parent = null;
                OnDied();
            }
        }
    }
}