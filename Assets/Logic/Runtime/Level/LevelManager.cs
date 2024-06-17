namespace Assets.Logic.Runtime.Level
{
    using Assets.Logic.Runtime.Pendulums;

    public class LevelManager
    {
        private readonly Pendulum Pendulum;

        public LevelManager(Pendulum pendulum)
        {
            Pendulum = pendulum;
        }

        public void RestartLevel()
        {
            GameContext.BallSpawnManager.ClearPendulumBall();
            GameContext.CanvasManager.CloseEndgameMenu();
            GameContext.ContainersManager.ClearAll();
            Pendulum.ResetAndEnable();
        }

        public void StartLevel()
        {
            GameContext.CanvasManager.CloseMainMenu();
            GameContext.BallSpawnManager.SpawnPendulumBall();
            GameContext.ScoreManager.ResetScore();
            Pendulum.ResetAndEnable();
        }

        public void FailLevel()
        {
            GameContext.BallSpawnManager.ClearPendulumBall();
            GameContext.CanvasManager.OpenEndgameMenu();
            GameContext.ContainersManager.ClearAll();
            Pendulum.Disable();
        }
    }
}