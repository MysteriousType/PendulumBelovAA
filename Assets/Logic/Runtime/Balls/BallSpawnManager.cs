namespace Assets.Logic.Runtime.Balls
{
    using System.Collections.Generic;
    using Assets.Logic.Runtime.Common.ObjectPooling;
    using UnityEngine;
    using UnityObject = UnityEngine.Object;

    public class BallSpawnManager
    {
        private readonly List<ObjectPool<Ball>> BallPools = new();
        private readonly GameObject BallSpawnPositionObject;

        public BallSpawnManager(GameObject ballSpawnPositionObject)
        {
            BallSpawnPositionObject = ballSpawnPositionObject;

            InitializePooling();

            StartSpawning(); // TEMPOOO!!!!
        }

        public void StartSpawning()
        {
            int ballPoolIndex = Random.Range(0, BallPools.Count);
            Ball ball = BallPools[ballPoolIndex].Pop();
            ball.IsSimulated = false;
            ball.AttachToParentObject(BallSpawnPositionObject);
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