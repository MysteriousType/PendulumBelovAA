namespace Assets.Logic.Runtime.Containers
{
    using Assets.Logic.Runtime.Balls;
    using Assets.Logic.Runtime.Time;

    public class ContainersManager
    {
        private const int MATRIX_SIZE = 3;
        private const float CHECK_MATCHES_PERIOD_DURATION_TIME = 0.88f;

        private readonly Ball[,] Balls = new Ball[MATRIX_SIZE, MATRIX_SIZE];
        private readonly ContainerTrigger BallTrigger;
        private readonly TimerEntity CheckMatchesPeriodTimer;

        public ContainersManager(ContainerTrigger ballTrigger)
        {
            BallTrigger = ballTrigger;
            BallTrigger.OnBallEntered += OnBallEntered;
            CheckMatchesPeriodTimer = new TimerEntity(CHECK_MATCHES_PERIOD_DURATION_TIME, onTimeIsUp:);
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
            bool matchColumn = HasColumnMatch(checkColumnIndex, ballId);
            bool matchRow = HasRowMatch(checkRowIndex, ballId);
            HasDiagonalMatches(checkColumnIndex, checkRowIndex, ballId, out bool matchLeftToRightDiagonal, out bool matchRightToLeftDiagonal);

            if (matchColumn)
            {
                ClearColumn(checkColumnIndex);
            }

            if (matchRow)
            {
                ClearRow(checkRowIndex);
            }

            if (matchLeftToRightDiagonal)
            {
                ClearDiagonal(true);
            }

            if (matchRightToLeftDiagonal)
            {
                ClearDiagonal(false);
            }
        }

        private bool HasColumnMatch(int checkColumnIndex, int ballId)
        {
            for (int rowIndex = 0; rowIndex < MATRIX_SIZE; rowIndex++)
            {
                Ball ball = Balls[checkColumnIndex, rowIndex];

                if (ball == null || ball.BallData.Id != ballId)
                {
                    return false;
                }
            }

            return true;
        }

        private bool HasRowMatch(int checkRowIndex, int ballId)
        {
            for (int columnIndex = 0; columnIndex < MATRIX_SIZE; columnIndex++)
            {
                Ball ball = Balls[columnIndex, checkRowIndex];

                if (ball == null || ball.BallData.Id != ballId)
                {
                    return false;
                }
            }

            return true;
        }

        private void HasDiagonalMatches(int checkColumnIndex, int checkRowIndex, int ballId, out bool matchLeftToRightDiagonal, out bool matchRightToLeftDiagonal)
        {
            if ((checkColumnIndex + checkRowIndex) % 2 != 0)
            {
                matchLeftToRightDiagonal = false;
                matchRightToLeftDiagonal = false;
                return;
            }

            matchLeftToRightDiagonal = CheckDiagonalLeftToRight(ballId);
            matchRightToLeftDiagonal = CheckDiagonalRightToLeft(ballId);
        }

        private bool CheckDiagonalLeftToRight(int ballId)
        {
            for (int index = 0; index < MATRIX_SIZE; index++)
            {
                Ball ball = Balls[index, index];

                if (ball == null || ball.BallData.Id != ballId)
                {
                    return false;
                }
            }

            return true;
        }

        private bool CheckDiagonalRightToLeft(int ballId)
        {
            int rowIndex = 0;

            for (int columnIndex = MATRIX_SIZE; columnIndex >= 0; columnIndex--)
            {
                Ball ball = Balls[columnIndex, rowIndex];

                if (ball == null || ball.BallData.Id != ballId)
                {
                    return false;
                }

                rowIndex++;
            }

            return true;
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