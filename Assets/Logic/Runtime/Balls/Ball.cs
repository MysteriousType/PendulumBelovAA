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

        private bool _isSimulated;

        public event Action<Ball> OnBallLanded;

        public bool IsSimulated
        {
            get => _isSimulated;
            set
            {
                _isSimulated = value;
                _rigidbody.simulated = value;
            }
        }

        public BallData BallData => _ballData;

        private void Update()
        {
            CheckIfOutOfBounds();
            CheckIfBallLanded();
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void CheckIfBallLanded()
        {
            const float ALMOST_ZERO_SPEED = 0f;

            if (OnBallLanded == null)
            {
                return;
            }

            if (_rigidbody.velocity.x <= ALMOST_ZERO_SPEED && _rigidbody.velocity.y <= ALMOST_ZERO_SPEED)
            {
                OnBallLanded?.Invoke(this);
            }
        }

        private void CheckIfOutOfBounds()
        {
            if (IsSimulated && transform.position.y <= -10f)
            {
                ReturnToPool(false);
            }
        }

        public void ReturnToPool(bool useParticleEffect)
        {
            _onReturnToPool?.Invoke(this);

            if (useParticleEffect)
            {
                SetupParticleEffect();
            }
        }

        private void SetupParticleEffect()
        {
            if (GameContext.BallParticleEffectsPool.TryGetParticleEffect(BallData.ParticleEffectName, out BallParticleEffect ballParticleEffect))
            {
                ballParticleEffect.transform.position = transform.position;
            }
        }

        public void AttachToParentObject(GameObject parentObject)
        {
            transform.parent = parentObject.transform;
            transform.localPosition = Vector3.zero;
        }

        public void SetVelocity(Vector2 velocity)
        {
            _rigidbody.velocity = velocity;
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
            _rigidbody.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }
}