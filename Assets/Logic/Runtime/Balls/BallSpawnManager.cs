﻿namespace Assets.Logic.Runtime.Balls
{
    using System.Collections.Generic;
    using Assets.Logic.Runtime.Common.ObjectPooling;
    using Assets.Logic.Runtime.Time;
    using UnityEngine;
    using UnityObject = UnityEngine.Object;

    public class BallSpawnManager
    {
        private const float BALL_SPAWN_DELAY_TIME = 0.5f;

        private readonly List<ObjectPool<Ball>> BallPools = new();
        private readonly GameObject BallSpawnPositionObject;
        private readonly TimerEntity BallSpawnDelayTimer;

        private Ball _pendulumBall;

        public BallSpawnManager(GameObject ballSpawnPositionObject)
        {
            BallSpawnPositionObject = ballSpawnPositionObject;
            BallSpawnDelayTimer = new TimerEntity(BALL_SPAWN_DELAY_TIME, onTimeIsUp: SpawnPendulumBall, startTimerOnCreation: false);

            InitializePooling();
        }

        public void OnLevelEnds()
        {
            if (_pendulumBall != null)
            {
                _pendulumBall.ReturnToPool(false);
            }

            BallSpawnDelayTimer.StopTimer();
        }

        public void OnLevelStarts()
        {
            BallSpawnDelayTimer.ResetTimer();
        }

        private void SpawnPendulumBall()
        {
            int ballPoolIndex = Random.Range(0, BallPools.Count);

            Ball ball = BallPools[ballPoolIndex].Pop();
            ball.IsSimulated = false;
            ball.AttachToParentObject(BallSpawnPositionObject);

            _pendulumBall = ball;
        }

        public void TryUnattachPendulumBall(Vector2 velocity)
        {
            if (_pendulumBall == null)
            {
                return;
            }

            _pendulumBall.IsSimulated = true;
            _pendulumBall.UnattachFromParent();
            _pendulumBall.SetVelocity(velocity);
            _pendulumBall = null;

            BallSpawnDelayTimer.ResetTimer();
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