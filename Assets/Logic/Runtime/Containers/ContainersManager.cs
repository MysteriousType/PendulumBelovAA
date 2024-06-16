﻿namespace Assets.Logic.Runtime.Containers
{
    using Assets.Logic.Runtime.Balls;

    public class ContainersManager
    {
        private const int MATRIX_SIZE = 3;
        private readonly Ball[,] Balls = new Ball[MATRIX_SIZE, MATRIX_SIZE];

        private ContainerTrigger _topTrigger;
        private ContainerTrigger _centerTrigger;
        private ContainerTrigger _bottomTrigger;

        public ContainersManager()
        {
            InitializeTriggers();
        }

        private void InitializeTriggers()
        {
            _topTrigger.OnBallEntered += OnBallLanded;
            _centerTrigger.OnBallEntered += OnBallLanded;
            _bottomTrigger.OnBallEntered += OnBallLanded;
        }

        private void OnBallLanded(Ball ball)
        {
            int columnIndex = GetColumnIndexByXPosiiton(ball.transform.position.x);

            for (int rowIndex = 0; rowIndex < MATRIX_SIZE; rowIndex++)
            {
                if (Balls[columnIndex, rowIndex] != null)
                {
                    Balls[columnIndex, rowIndex] = ball;
                    break;
                }
            }
        }

        private int GetColumnIndexByXPosiiton(float xPosition)
        {
            if (xPosition > -1.074f && xPosition < -0.36f)
            {
                return 0;
            }

            if (xPosition > -0.36f && xPosition < 0.36f)
            {
                return 1;
            }

            /*
            if (xPosition > 0.36f && xPosition < 1.074f)
            {
                return 2;
            }
            */

            return 2;
        }
    }
}