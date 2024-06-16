namespace Assets.Logic.Runtime.Balls
{
    using Assets.Logic.Runtime.Balls.Data;
    using Assets.Logic.Runtime.Common.ObjectPooling;
    using System;
    using UnityEngine;

    public class Ball : MonoBehaviour, IPoolable<Ball>
    {
        [SerializeField]
        private BallData _ballData;

        private Action<Ball> _onReturnToPool;
        private Rigidbody2D _rigidbody;

        public bool IsSimulated
        {
            set
            {
                _rigidbody.simulated = value;
            }
        }

        public BallData BallData => _ballData;

        public void AttachToParentObject(GameObject parentObject)
        {
            transform.parent = parentObject.transform;
            transform.localPosition = Vector3.zero;
        }

        public void UnattachFromParent()
        {
            transform.parent = null;
        }

        public void Initialize(Action<Ball> onReturnToPool)
        {
            _onReturnToPool = onReturnToPool;
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void OnPopped()
        {
            gameObject.SetActive(true);
        }

        public void OnPushed()
        {
            gameObject.SetActive(false);
        }
    }
}