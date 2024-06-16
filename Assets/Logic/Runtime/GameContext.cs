namespace Assets.Logic.Runtime
{
    using UnityEngine;

    public static class GameContext
    {
        public static void Initialize()
        {
            InitializeQuality();
            InitializeCameraSize();
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
    }
}