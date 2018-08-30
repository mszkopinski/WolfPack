using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Wolfpack
{
    public class ObstacleFormationsSpawner : MonoSingleton<ObstacleFormationsSpawner>
    {
        public bool ShouldSpawnImmediately;
        
        [Header("Obstacles")]
        [SerializeField] GameObject obstaclePrefab;
        [SerializeField] float firstFormationDistanceFromZero = 30f;
        [SerializeField] int formationsToSpawn = 10;
        [SerializeField] float minDistanceBetweenFormations = 15f;
        [SerializeField] float maxDistanceBetweenFormations = 25f;
        
        [SerializeField] float minObstacleHeight;
        [SerializeField] float maxObstacleHeight = 25f;
        
        [Header("Chances")]
        [SerializeField] float chanceToSpawnFormation = 0.8f;

        [SerializeField, Range(0, 100)] int chanceToSpawnOneObstacle = 50;
        [SerializeField, Range(0, 100)] int chanceToSpawnTwoObstacles = 40;
        [SerializeField, Range(0, 100)] int chanceToSpawnThreeObstacles = 10;

        float lastFormationPosition;
        readonly List<ObstacleFormation> ObstacleFormations = new List<ObstacleFormation>();

        void Start()
        {
            if (ShouldSpawnImmediately) Spawn();
        }

        public void Spawn()
        {
            lastFormationPosition = firstFormationDistanceFromZero;

            for (var i = 0; i < formationsToSpawn; i++)
            {
                var obstacleFormation = new ObstacleFormation(
                    GetRandomObstaclesNumber(), 
                    lastFormationPosition, 
                    minObstacleHeight, 
                    maxObstacleHeight);
                
                SpawnFormation(obstacleFormation);
                ObstacleFormations.Add(obstacleFormation);
                lastFormationPosition += Random.Range(minDistanceBetweenFormations, maxDistanceBetweenFormations);
            }
        }

        void SpawnFormation(ObstacleFormation obstacleFormation)
        {
            for (var i = 0; i < obstacleFormation.ObstaclesNumber; i++)
            {
                var obstaclePosition = new Vector3(
                    MovementHelper.LinePositions[obstacleFormation.OccupiedLines.ElementAt(i)],
                    Random.Range(obstacleFormation.MinObstacleHeight, obstacleFormation.MaxObstacleHeight),
                    obstacleFormation.Distance);
                Instantiate(obstaclePrefab, obstaclePosition, Quaternion.identity);
            }
        }

        public ObstacleFormation GetNearestFormation(Transform target)
        {
            var nearestFormation = ObstacleFormations.Last();
            var targetDistance = target.position.z;

            foreach (var formation in ObstacleFormations)
            {
                var distanceBetweenCurrentFormation = Math.Abs(formation.Distance - targetDistance);
                var distanceBetweenCurrentNearestFormation = Math.Abs(nearestFormation.Distance - targetDistance);
                if (distanceBetweenCurrentFormation < distanceBetweenCurrentNearestFormation)
                    nearestFormation = formation;
            }
            
            return nearestFormation;
        }

        int GetRandomObstaclesNumber()
        {
            var obstaclesInFormation = 0;

            if (Random.Range(0, 100) > 100 - chanceToSpawnFormation)
                obstaclesInFormation = 0;
            else if (Random.Range(0, 100) > 100 - chanceToSpawnOneObstacle)
                obstaclesInFormation = 1;
            else if (Random.Range(0, 100) > 100 - chanceToSpawnTwoObstacles)
                obstaclesInFormation = 2;

            return obstaclesInFormation;
        }
    }

    public class ObstacleFormation
    {
        public readonly int ObstaclesNumber;
        public readonly float Distance;
        public readonly float MinObstacleHeight;
        public readonly float MaxObstacleHeight;
        public readonly IReadOnlyCollection<Line> OccupiedLines;

        public ObstacleFormation(int obstaclesNumber, float distance, float minObstacleHeight, float maxObstacleHeight)
        {
            ObstaclesNumber = obstaclesNumber;
            Distance = distance;
            MinObstacleHeight = minObstacleHeight;
            MaxObstacleHeight = maxObstacleHeight;
            OccupiedLines = GetOccupiedLines();
        }

        IReadOnlyCollection<Line> GetOccupiedLines()
        {
            var previousLine = MovementHelper.GetRandomLine();
            var lines = new List<Line>();
            for (int i = 0; i < ObstaclesNumber; i++)
                lines.Add(i == 0 ? previousLine : GetRandomSpawnLine(previousLine));
            return lines;
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
