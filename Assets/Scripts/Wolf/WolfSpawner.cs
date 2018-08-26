using System.Collections;
using UnityEngine;

namespace Wolfpack
{
    public class WolfSpawner : MonoSingleton<WolfSpawner>
    {
        [Header("Spawn Settings")] 
        [SerializeField] float minTimeBetweenSpawns = 1f;
        [SerializeField] float maxTimeBetweenSpawns = 3f;
    
        [SerializeField] Transform[] spawnPoints;
        [SerializeField] GameObject wolfPrefab;

        void Start()
        {
            EnableSpawner();
        }
    
        public void EnableSpawner()
        {
            StartCoroutine(SpawnWolfsCoroutine());
        }

        IEnumerator SpawnWolfsCoroutine()
        {
            while (true)
            {
                var spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                SpawnWolf(spawnPoint);
                yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
            }
        }

        void SpawnWolf(Transform spawnPoint)
        {
            var wolf = Instantiate(wolfPrefab, spawnPoint);
            wolf.transform.parent = null;
        }
    }
}