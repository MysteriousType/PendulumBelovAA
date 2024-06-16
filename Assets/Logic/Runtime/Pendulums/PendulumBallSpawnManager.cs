namespace Assets.Logic.Runtime.Pendulums
{
    using Assets.Logic.Runtime.Balls;
    using Assets.Logic.Runtime.Common.ObjectPooling;
    using System.Collections.Generic;

    public class PendulumBallSpawnManager
    {
        private readonly Dictionary<int, ObjectPool<Ball>> BallsPoolByBallId;

        public PendulumBallSpawnManager()
        {

        }
    }
}