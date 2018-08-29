using UnityEngine;
using Wolfpack;

namespace WolfPack
{
    public class ObstacleFormationsSpawner : MonoSingleton<ObstacleFormationsSpawner>
    {
        [Header("Obstacles")]
        [SerializeField] GameObject obstaclePrefab;
        [SerializeField] float firstFormationDistanceFromZero = 30f;
        [SerializeField] int formationsToSpawn = 10;
        [SerializeField] float minDistanceBetweenFormations = 15f;
        [SerializeField] float maxDistanceBetweenFormations = 25f;
        
        [SerializeField] float minObstacleHeight = 0f;
        [SerializeField] float maxObstacleHeight = 25f;
        
        [Header("Chances")]
        [SerializeField] float chanceToSpawnFormation = 0.8f;

        [SerializeField, Range(0, 100)] int chanceToSpawnOneObstacle = 50;
        [SerializeField, Range(0, 100)] int chanceToSpawnTwoObstacles = 40;
        [SerializeField, Range(0, 100)] int chanceToSpawnThreeObstacles = 10;

        float currentFormationPosition;
        
        public void Spawn()
        {
            currentFormationPosition = firstFormationDistanceFromZero;

            for (int i = 0; i < formationsToSpawn; i++)
            {
                var obstaclesInFormation = GetObstaclesNumberInFormation();
                var randomLine = MovementHelper.GetRandomLine();
                
                SpawnFormation(obstaclesInFormation, currentFormationPosition, randomLine);

                currentFormationPosition += Random.Range(minDistanceBetweenFormations, maxDistanceBetweenFormations);
            }
        }

        void SpawnFormation(int number, float distance, Line firstLine)
        {
            Log.Console($"Spawning formation of {number}");
            var lastLine = firstLine;
            
            for (int i = 0; i < number; i++)
            {
                var obstaclePosition = new Vector3(
                    MovementHelper.LinePositions[i == 0 ? lastLine : GetRandomSpawnLine(lastLine)],
                    Random.Range(minObstacleHeight, maxObstacleHeight),
                    distance);
                Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
            }
        }

        int GetObstaclesNumberInFormation()
        {
            var obstaclesInFormation = 0;

            if (Random.Range(0, 100) > 100 - chanceToSpawnFormation)
                obstaclesInFormation = 0;
            else if (Random.Range(0, 100) > 100 - chanceToSpawnOneObstacle)
                obstaclesInFormation = 1;
            else if (Random.Range(0, 100) > 100 - chanceToSpawnTwoObstacles)
                obstaclesInFormation = 2;
            else if (Random.Range(0, 100) > 100 - chanceToSpawnThreeObstacles)
                obstaclesInFormation = 3;

            return obstaclesInFormation;
        }
        
        Line GetRandomSpawnLine(Line previousLine)
        {
            var newLine = previousLine;
            while (newLine == previousLine)
                newLine = MovementHelper.GetRandomLine();
            return newLine;
        }
    }
}
