using System.Collections;
using System.Linq;
using UnityEngine;

namespace Wolfpack
{
    public class WolfSpawner : MonoSingleton<WolfSpawner>
    {
        [Header("Spawn Settings")] 
        [SerializeField] float minTimeBetweenSpawns = 1f;
        [SerializeField] float maxTimeBetweenSpawns = 3f;
    
        [SerializeField] GameObject wolfPrefab;
    
        public void StartSpawner()
        {
            StartCoroutine(ConstantlySpawnWolfs());
        }

        IEnumerator ConstantlySpawnWolfs()
        {
            while (true)
            {
                Instantiate(
                    wolfPrefab, 
                    new Vector3(0f, 0f, 
                        MovementHelper.Lines.ElementAt(Random.Range(0, MovementHelper.Lines.Count)).Value), 
                    Quaternion.identity);
                yield return new WaitForSeconds(Random.Range(minTimeBetweenSpawns, maxTimeBetweenSpawns));
            }
        }
    }
}