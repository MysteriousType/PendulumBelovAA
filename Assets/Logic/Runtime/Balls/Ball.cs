namespace Assets.Logic.Runtime.Balls
{
    using Assets.Logic.Runtime.Balls.Data;
    using UnityEngine;

    public class Ball : MonoBehaviour
    {
        [SerializeField]
        private BallData _ballData;

        public BallData BallData => _ballData;
    }
}