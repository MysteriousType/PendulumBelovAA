namespace Assets.Logic.Runtime
{
    using Assets.Logic.Runtime.Balls;
    using Assets.Logic.Runtime.Canvas;
    using Assets.Logic.Runtime.Containers;
    using Assets.Logic.Runtime.Level;
    using Assets.Logic.Runtime.Pendulums;
    using Assets.Logic.Runtime.Providers;
    using Assets.Logic.Runtime.Score;
    using Assets.Logic.Runtime.Time;
    using UnityEngine;
    using UnityObject = UnityEngine.Object;

    public static class GameContext
    {
        public static PrefabsProvider PrefabsProvider { get; private set; }
        public static BallSpawnManager BallSpawnManager { get; private set; }
        public static TimeManager TimeManager { get; private set; }
        public static ContainersManager ContainersManager { get; private set; }
        public static ScoreManager ScoreManager { get; private set; }
        public static BallParticleEffectsPool BallParticleEffectsPool { get; private set; }
        public static CanvasManager CanvasManager { get; private set; }
        public static LevelManager LevelManager { get; private set; }

        public static void Initialize()
        {
            PrefabsProvider = new PrefabsProvider();

            InitializeTime();
            InitializeQuality();
            InitializeCameraSize();
            InitializeBallSpawnManager();
            InitializeContainersManager();
            InitializeCanvasManager();
            InitializeLevelManager();

            ScoreManager = new ScoreManager();
            BallParticleEffectsPool = new BallParticleEffectsPool();
        }

        private static void InitializeLevelManager()
        {
            Pendulum pendulum = GameObject.Find("PendulumHand").GetComponent<Pendulum>();
            LevelManager = new LevelManager(pendulum);
        }

        private static void InitializeCanvasManager()
        {
            CanvasManager = new CanvasManager();
            CanvasManager.OpenMainMenu();
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