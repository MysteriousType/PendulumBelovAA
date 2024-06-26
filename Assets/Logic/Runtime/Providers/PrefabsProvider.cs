﻿namespace Assets.Logic.Runtime.Providers
{
    using Assets.Logic.Runtime.Balls;
    using Assets.Logic.Runtime.Time;
    using System.Collections.Generic;
    using UnityEngine;

    public class PrefabsProvider
    {
        public IReadOnlyList<Ball> BallPrefabs { get; private set; }
        public IReadOnlyList<BallParticleEffect> BallParticleEffects { get; private set; }
        public TimeManager TimeManager { get; private set; }

        public PrefabsProvider()
        {
            LoadTimeManager();
            LoadBalls();
            LoadBallParticleEffects();
        }

        private void LoadBalls()
        {
            string[] prefabNames = new string[]
            {
                "Ball1",
                "Ball2",
                "Ball3",
            };

            List<Ball> balls = new();

            foreach (string prefabName in prefabNames)
            {
                string path = $"Prefabs/Balls/{prefabName}";
                Ball ball = LoadPrefab(path).GetComponent<Ball>();
                balls.Add(ball);
            }

            BallPrefabs = balls;
        }

        private void LoadBallParticleEffects()
        {
            string[] prefabNames = new string[]
            {
                "BallParticleEffectRed",
                "BallParticleEffectGreen",
                "BallParticleEffectBlue",
            };

            List<BallParticleEffect> ballParticleEffects = new();

            foreach (string prefabName in prefabNames)
            {
                string path = $"Prefabs/Particles/{prefabName}";
                BallParticleEffect ballParticleEffect = LoadPrefab(path).GetComponent<BallParticleEffect>();
                ballParticleEffects.Add(ballParticleEffect);
            }

            BallParticleEffects = ballParticleEffects;
        }

        // ДОБАВИТЬ ПРОИГРЫШ!!!

        private void LoadTimeManager()
        {
            const string PATH = "Prefabs/Managers/TimeManager";
            TimeManager = LoadPrefab(PATH).GetComponent<TimeManager>();
        }

        private GameObject LoadPrefab(string path)
        {
            return Resources.Load<GameObject>(path);
        }
    }
}