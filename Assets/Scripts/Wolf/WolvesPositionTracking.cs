using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wolfpack
{
    public class WolvesPositionTracking : MonoSingleton<WolvesPositionTracking>
    {
        [SerializeField] float checkPositionsEverySeconds = 5f;

        public List<Wolf> FastestWolves { get; private set; }
        IReadOnlyCollection<Wolf> currentlyRunningWolves;
    
        void Start()
        {
            FastestWolves = new List<Wolf>();
            GameManager.Instance.WolfNumberChanged += CheckForPositionChanges;
            ConstantlyCheckForPositionChanges().RunWithDelay(0.1f);
        }

        IEnumerator ConstantlyCheckForPositionChanges()
        {
            while (true)
            {
                CheckForPositionChanges();
                yield return new WaitForSeconds(checkPositionsEverySeconds);
            }
        }

        void CheckForPositionChanges()
        {
//            currentlyRunningWolves = GameManager.Instance.Wolves.Select(wolf => wolf).ToList();
            
//            FastestWolves.Clear();
//            FastestWolves = GameManager.Instance.Wolves
//                .OrderBy(wolf => wolf.transform.position.z)
//                .Skip(Math.Max(0, GameManager.Instance.Wolves.Count - 3))
//                .Reverse()
//                .ToList();
//            
            for (var i = 0; i < FastestWolves.Count; i++)
                FastestWolves[i].SendMessage("SetRacePosition", i + 1, SendMessageOptions.DontRequireReceiver);
        }
    }
}