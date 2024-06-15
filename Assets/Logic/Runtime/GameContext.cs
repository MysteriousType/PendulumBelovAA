namespace Assets.Logic.Runtime
{
    using UnityEngine;

    public static class GameContext
    {
        public static void Initialize()
        {
            InitializeQuality();
            InitializeCamera();
        }

        private static void InitializeQuality()
        {
            const int FRAME_RATE_LOCK = 60;
            Application.targetFrameRate = FRAME_RATE_LOCK;
        }

        private static void InitializeCamera()
        {
            Camera camera = Camera.main;

            float w = 5;
            float h = 10;
            float x = w * 0.5f - 0.5f;
            float y = h * 0.5f - 0.5f;

            //camera.transform.position = new Vector3(x, y, -10f);
            camera.orthographicSize = ((w > h * camera.aspect) ? (float)w / (float)camera.pixelWidth * camera.pixelHeight : h) / 2f;
        }
    }
}