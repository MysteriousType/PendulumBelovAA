namespace Assets.Logic.Runtime
{
    using Assets.Logic.Runtime.Balls;
    using Assets.Logic.Runtime.Containers;
    using Assets.Logic.Runtime.Providers;
    using Assets.Logic.Runtime.Time;
    using UnityEngine;
    using UnityObject = UnityEngine.Object;

    public static class GameContext
    {
        public static PrefabsProvider PrefabsProvider { get; private set; }
        public static BallSpawnManager BallSpawnManager { get; private set; }
        public static TimeManager TimeManager { get; private set; }
        public static ContainersManager ContainersManager { get; private set; }

        public static void Initialize()
        {
            PrefabsProvider = new PrefabsProvider();

            InitializeTime();
            InitializeQuality();
            InitializeCameraSize();
            InitializeBallSpawnManager();
            InitializeContainersManager();
        }

        private static void InitializeTime()
        {
            TimeManager = UnityObject.Instantiate(PrefabsProvider.TimeManager);
            UnityObject.DontDestroyOnLoad(TimeManager);
        }

        private static void InitializeQuality()
        {
            const int FRAME_RATE_LOCK = 60;
            Application.targetFrameRate = FRAME_RATE_LOCK;
        }

        private static void InitializeCameraSize()
        {
            const float SCENE_WIDTH = 1080f / 100f * 0.4f;
            const float SCENE_HEIGHT = 1920f / 100f * 0.4f;

            Camera camera = Camera.main;
            camera.orthographicSize = ((SCENE_WIDTH > SCENE_HEIGHT * camera.aspect) ? SCENE_WIDTH / camera.pixelWidth * camera.pixelHeight : SCENE_HEIGHT) * 0.5f;
        }

        private static void InitializeBallSpawnManager()
        {
            GameObject ballSpawnPositionObject = GameObject.Find("BallSpawnPosition");
            BallSpawnManager = new BallSpawnManager(ballSpawnPositionObject);
        }

        private static void InitializeContainersManager()
        {
            ContainerTrigger trigger = GameObject.Find("ContainerBallTrigger").GetComponent<ContainerTrigger>();
            ContainersManager = new ContainersManager(trigger);
        }
    }
}