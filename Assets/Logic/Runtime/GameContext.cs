namespace Assets.Logic.Runtime
{
    using Assets.Logic.Runtime.Balls;
    using Assets.Logic.Runtime.Providers;
    using UnityEngine;

    public static class GameContext
    {
        public static PrefabsProvider PrefabsProvider { get; private set; }
        public static BallSpawnManager BallSpawnManager { get; private set; }

        public static void Initialize()
        {
            PrefabsProvider = new PrefabsProvider();

            InitializeQuality();
            InitializeCameraSize();
            InitializeBallSpawnManager();

        }

        private static void InitializeQuality()
        {
            const int FRAME_RATE_LOCK = 60;
            Application.targetFrameRate = FRAME_RATE_LOCK;
        }

        private static void InitializeCameraSize()
        {
            const float SCENE_WIDTH = 5f;
            const float SCENE_HEIGHT = 10f;

            Camera camera = Camera.main;
            camera.orthographicSize = ((SCENE_WIDTH > SCENE_HEIGHT * camera.aspect) ? SCENE_WIDTH / camera.pixelWidth * camera.pixelHeight : SCENE_HEIGHT) * 0.5f;
        }

        private static void InitializeBallSpawnManager()
        {
            GameObject ballSpawnPositionObject = GameObject.Find("BallSpawnPosition");
            BallSpawnManager = new BallSpawnManager(ballSpawnPositionObject);
        }
    }
}