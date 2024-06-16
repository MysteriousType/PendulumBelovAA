namespace Assets.Logic.Runtime.Pendulums
{
    using UnityEngine;

    public class Pendulum : MonoBehaviour
    {
        private const float MOVE_SPEED = 25f;
        private const float MOVE_SPEED_MULTIPLIER = 150f;
        private const float MAXIMUM_ANGLE = 0.5f;
        private const float MAXIMUM_RIGHT_ANGLE = MAXIMUM_ANGLE;
        private const float MAXIMUM_LEFT_ANGLE = MAXIMUM_ANGLE * -1f;

        private Rigidbody2D _rigidbody;
        private bool _isMovingClockwise = true;

        private float Angle => transform.rotation.z;

        private float AbsAngle => Mathf.Abs(Angle);

        private bool IsMovingClockwise
        {
            get
            {
                if (Angle > MAXIMUM_RIGHT_ANGLE)
                {
                    _isMovingClockwise = false;
                }

                if (Angle < MAXIMUM_LEFT_ANGLE)
                {
                    _isMovingClockwise = true;
                }

                return _isMovingClockwise;
            }
        }

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Update()
        {
            CheckTouch();
        }

        private void CheckTouch()
        {
            const float VELOCITY_MULTIPLIER = 3.5f;

            if (Input.touchCount > 0)
            {
                GameContext.BallSpawnManager.TryUnattachPendulumBall(_rigidbody.velocity * VELOCITY_MULTIPLIER);
            }
        }

        private void Move()
        {
            float additionalSpeed = (MAXIMUM_ANGLE - AbsAngle) * MOVE_SPEED_MULTIPLIER;
            float moveSpeed = MOVE_SPEED + additionalSpeed;

            if (IsMovingClockwise)
            {
                _rigidbody.angularVelocity = moveSpeed;
            }
            else
            {
                _rigidbody.angularVelocity = moveSpeed * -1f;
            }
        }
    }
}