namespace Assets.Logic.Runtime.Containers
{
    using Assets.Logic.Runtime.Balls;

    public class ContainersManager
    {
        private const int MATRIX_SIZE = 3;
        private readonly Ball[,] Balls = new Ball[MATRIX_SIZE, MATRIX_SIZE];
        private readonly ContainerTrigger BallTrigger;

        public ContainersManager(ContainerTrigger ballTrigger)
        {
            BallTrigger = ballTrigger;
            BallTrigger.OnBallEntered += OnBallEntered;
        }

        private void OnBallEntered(Ball ball)
        {
            int columnIndex = GetColumnIndexByXPosiiton(ball.transform.position.x);

            for (int rowIndex = MATRIX_SIZE - 1; rowIndex >= 0; rowIndex--)
            {
                if (Balls[columnIndex, rowIndex] == null)
                {
                    Balls[columnIndex, rowIndex] = ball;
                    CheckMatches(columnIndex, rowIndex, ball.BallData.Id);
                    break;
                }
            }
        }

        private void CheckMatches(int checkColumnIndex, int checkRowIndex, int ballId)
        {
            CheckColumnForMatch(checkColumnIndex, ballId);
            CheckRowForMatch(checkRowIndex, ballId);
            CheckDiagonalForMatch(checkColumnIndex, checkRowIndex, ballId);
        }

        private void CheckColumnForMatch(int checkColumnIndex, int ballId)
        {
            for (int rowIndex = 0; rowIndex < MATRIX_SIZE; rowIndex++)
            {
                Ball ball = Balls[checkColumnIndex, rowIndex];

                if (ball == null || ball.BallData.Id != ballId)
                {
                    return;
                }
            }

            ClearColumn(checkColumnIndex);
        }

        private void CheckRowForMatch(int checkRowIndex, int ballId)
        {
            for (int columnIndex = 0; columnIndex < MATRIX_SIZE; columnIndex++)
            {
                Ball ball = Balls[columnIndex, checkRowIndex];

                if (ball == null || ball.BallData.Id != ballId)
                {
                    return;
                }
            }

            ClearRow(checkRowIndex);
        }

        private void CheckDiagonalForMatch(int checkColumnIndex, int checkRowIndex, int ballId)
        {
            if ((checkColumnIndex + checkRowIndex) % 2 != 0)
            {
                return;
            }

            CheckDiagonalLeftToRight(ballId);
            CheckDiagonalRightToLeft(ballId);
        }

        private void CheckDiagonalLeftToRight(int ballId)
        {
            for (int index = 0; index < MATRIX_SIZE; index++)
            {
                Ball ball = Balls[index, index];

                if (ball == null || ball.BallData.Id != ballId)
                {
                    return;
                }
            }

            ClearDiagonal(true);
        }

        private void CheckDiagonalRightToLeft(int ballId)
        {
            int rowIndex = 0;

            for (int columnIndex = MATRIX_SIZE; columnIndex >= 0; columnIndex--)
            {
                Ball ball = Balls[columnIndex, rowIndex];

                if (ball == null || ball.BallData.Id != ballId)
                {
                    return;
                }

                rowIndex++;
            }

            ClearDiagonal(false);
        }

        private void ClearDiagonal(bool isLeftToRight)
        {
            RemoveBallFromMatrix(1, 1);

            if (isLeftToRight)
            {
                RemoveBallFromMatrix(0, 0);
                RemoveBallFromMatrix(2, 2);
                return;
            }

            RemoveBallFromMatrix(0, 2);
            RemoveBallFromMatrix(2, 0);
        }

        private void ClearColumn(int columnIndex)
        {
            for (int rowIndex = 0; rowIndex < MATRIX_SIZE; rowIndex++)
            {
                RemoveBall(columnIndex, rowIndex);
            }
        }

        private void ClearRow(int rowIndex)
        {
            for (int columnIndex = 0; columnIndex < MATRIX_SIZE; columnIndex++)
            {
                RemoveBallFromMatrix(columnIndex, rowIndex);
            }
        }

        private void RemoveBallFromMatrix(int removeAtColumnIndex, int removeAtRowIndex)
        {
            for (int rowIndex = removeAtRowIndex; rowIndex >= 1; rowIndex--)
            {
                Balls[removeAtColumnIndex, rowIndex] = Balls[removeAtColumnIndex, rowIndex--];
            }

            // If we not got into "for" body, then removeAtRowIndex is 0
            RemoveBall(removeAtColumnIndex, 0);
        }

        private void RemoveBall(int removeAtColumnIndex, int removeAtRowIndex)
        {
            Balls[removeAtColumnIndex, removeAtRowIndex].DestroyWithDelay();
            Balls[removeAtColumnIndex, removeAtRowIndex] = null;
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