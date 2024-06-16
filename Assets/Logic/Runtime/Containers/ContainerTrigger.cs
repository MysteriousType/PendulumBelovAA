namespace Assets.Logic.Runtime.Containers
{
    using Assets.Logic.Runtime.Balls;
    using System;
    using UnityEngine;

    public class ContainerTrigger : MonoBehaviour
    {
        public event Action<Ball> OnBallEntered;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Ball ball = collision.GetComponent<Ball>();
            OnBallEntered?.Invoke(ball);
        }
    }
}