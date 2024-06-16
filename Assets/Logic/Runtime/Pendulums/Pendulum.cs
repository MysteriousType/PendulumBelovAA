namespace Assets.Logic.Runtime.Pendulums
{
    using UnityEngine;

    public class Pendulum : MonoBehaviour
    {
        private const float MOVE_SPEED = 40f;
        private const float MAXIMUM_RIGHT_ANGLE = 0.35f;
        private const float MAXIMUM_LEFT_ANGLE = MAXIMUM_RIGHT_ANGLE * -1f;

        private Rigidbody2D _rigidbody2D;
        private bool _isMovingClockwise = true;

        private bool IsMovingClockwise
        {
            get
            {
                if (transform.rotation.z > MAXIMUM_RIGHT_ANGLE)
                {
                    _isMovingClockwise = false;
                }

                if (transform.rotation.z < MAXIMUM_LEFT_ANGLE)
                {
                    _isMovingClockwise = true;
                }

                return _isMovingClockwise;
            }
        }

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            if (IsMovingClockwise)
            {
                _rigidbody2D.angularVelocity = MOVE_SPEED;
            }
            else
            {
                _rigidbody2D.angularVelocity = MOVE_SPEED * -1f;
            }
        }
    }
}