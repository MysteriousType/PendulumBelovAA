namespace Assets.Logic.Runtime.Providers
{
    using Assets.Logic.Runtime.Balls;
    using System.Collections.Generic;
    using UnityEngine;

    public class PrefabsProvider
    {
        public IReadOnlyList<Ball> BallPrefabs { get; private set; }

        public PrefabsProvider()
        {
            LoadBalls();
        }

        private void LoadBalls()
        {
            string[] prefabNames = new string[]
            {
                "Ball1",
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

        private GameObject LoadPrefab(string path)
        {
            return Resources.Load<GameObject>(path);
        }
    }
}