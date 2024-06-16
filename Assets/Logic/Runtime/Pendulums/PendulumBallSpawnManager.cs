namespace Assets.Logic.Runtime.Pendulums
{
    using System.Collections.Generic;
    using Assets.Logic.Runtime.Balls;
    using Assets.Logic.Runtime.Common.ObjectPooling;
    using UnityEngine;
    using UnityObject = UnityEngine.Object;

    public class PendulumBallSpawnManager
    {
        private readonly List<ObjectPool<Ball>> BallPools = new();

        public PendulumBallSpawnManager()
        {
            InitializePooling();

            StartSpawning(); // TEMPOOO!!!!
        }

        public void StartSpawning()
        {
            int ballPoolIndex = Random.Range(0, BallPools.Count);
            Ball ball = BallPools[ballPoolIndex].Pop();
        }

        private void InitializePooling()
        {
            foreach (Ball ballPrefab in GameContext.PrefabsProvider.BallPrefabs)
            {
                ObjectPool<Ball> ballPool = new(() => CreateBallObject(ballPrefab));
                BallPools.Add(ballPool);
            }

            static Ball CreateBallObject(Ball ballPrefab)
            {
                return UnityObject.Instantiate(ballPrefab);
            }
        }
    }
}