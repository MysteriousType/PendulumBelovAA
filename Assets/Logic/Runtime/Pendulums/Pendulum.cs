namespace Assets.Logic.Runtime.Pendulums
{
    using UnityEngine;

    public class Pendulum : MonoBehaviour
    {
        private const float MOVE_SPEED = 40f;

        private Rigidbody2D _rigidbody2D;

        private bool _isMovingClockwise = true;
        private float _rightAngle = 0.35f;
        private float _leftAngle = -0.35f;

        private void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            Move();
        }

        private void CheckIfClockwise()
        {
            if (transform.rotation.z > _rightAngle)
            {
                _isMovingClockwise = false;
            }

            if (transform.rotation.z < _leftAngle)
            {
                _isMovingClockwise = true;
            }
        }

        private void Move()
        {
            CheckIfClockwise();

            if (_isMovingClockwise)
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