using System.Collections;
using System.Linq;
using UnityEngine;

namespace Wolfpack
{
    public class WolfSpawner : MonoSingleton<WolfSpawner>
    {
        public bool ShouldSpawnImmediately;
        
        [Header("Spawn Settings")] 
        [SerializeField] float minTimeBetweenSpawns = 1f;
        [SerializeField] float maxTimeBetweenSpawns = 3f;
    
        [SerializeField] GameObject wolfPrefab;

        void Start()
        {
            if (ShouldSpawnImmediately) StartSpawning().Run();
        }
    
        public IEnumerator StartSpawning()
        {
            while (true)
            {
                var randomLine = MovementHelper.GetRandomLine();
                var wolf = Instantiate(
                    wolfPrefab, 
                    new Vector3(0f, 0f, MovementHelper.LinePositions[randomLine]), Quaternion.identity);
                wolf.GetComponent<AIWolfMovementController>().CurrentLine = randomLine;
                yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
            }
        }
    }
}