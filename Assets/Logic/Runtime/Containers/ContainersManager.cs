namespace Assets.Logic.Runtime.Containers
{
    using Assets.Logic.Runtime.Balls;
    using Assets.Logic.Runtime.Time;

    public class ContainersManager
    {
        private const int MATRIX_SIZE = 3;
        private const float CHECK_MATCHES_PERIOD_DURATION_TIME = 0.6f;

        private readonly Ball[,] Balls = new Ball[MATRIX_SIZE, MATRIX_SIZE];
        private readonly ContainerTrigger BallTrigger;
        private readonly TimerEntity DoubleCheckTimer;

        public ContainersManager(ContainerTrigger ballTrigger)
        {
            BallTrigger = ballTrigger;
            BallTrigger.OnBallEntered += OnBallEntered;

            // Invoke timer only if any diagonal was removed! Check only rows in this case (except for row with index 0)
            DoubleCheckTimer = new TimerEntity(CHECK_MATCHES_PERIOD_DURATION_TIME, onTimeIsUp: PerformDoubleCheck, startTimerOnCreation: false);
        }

        private void OnBallEntered(Ball ball)
        {
            ball.OnBallLanded += OnBallLanded;
        }

        private void OnBallLanded(Ball ball)
        {
            ball.OnBallLanded -= OnBallLanded;

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

            /*
            UnityEngine.Debug.Log("=======");
            UnityEngine.Debug.Log($"{Balls[0, 0]?.BallData.Id}|{Balls[1, 0]?.BallData.Id}|{Balls[2, 0]?.BallData.Id}");
            UnityEngine.Debug.Log($"{Balls[0, 1]?.BallData.Id}|{Balls[1, 1]?.BallData.Id}|{Balls[2, 1]?.BallData.Id}");
            UnityEngine.Debug.Log($"{Balls[0, 2]?.BallData.Id}|{Balls[1, 2]?.BallData.Id}|{Balls[2, 2]?.BallData.Id}");
            UnityEngine.Debug.Log("=======");
            */
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

            bool needToShiftBalls = matchRow || matchLeftToRightDiagonal || matchRightToLeftDiagonal;

            if (needToShiftBalls)
            {
                ShiftBallsInMatrix();
            }

            bool needToDoubleCheck = matchLeftToRightDiagonal || matchRightToLeftDiagonal;

            if (needToDoubleCheck)
            {
                DoubleCheckTimer.ResetTimer();
            }
        }

        private void PerformDoubleCheck()
        {
            const int ROW_INDEX_1 = 1;
            const int ROW_INDEX_2 = 2;
            const int COLUMN_INDEX_DUMMY = 0;

            int? ballIdRowIndex1 = Balls[COLUMN_INDEX_DUMMY, ROW_INDEX_1]?.BallData.Id;
            int? ballIdRowIndex2 = Balls[COLUMN_INDEX_DUMMY, ROW_INDEX_2]?.BallData.Id;

            bool hasMatchRowIndex1 = ballIdRowIndex1.HasValue && HasRowMatch(1, ballIdRowIndex1.Value);
            bool hasMatchRowIndex2 = ballIdRowIndex2.HasValue && HasRowMatch(2, ballIdRowIndex2.Value);
            bool needToShiftBalls = hasMatchRowIndex1 || hasMatchRowIndex2;

            if (hasMatchRowIndex1)
            {
                ClearRow(ROW_INDEX_1);
            }

            if (hasMatchRowIndex2)
            {
                ClearRow(ROW_INDEX_2);
            }

            if (needToShiftBalls)
            {
                ShiftBallsInMatrix();
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

            for (int columnIndex = MATRIX_SIZE - 1; columnIndex >= 0; columnIndex--)
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
            RemoveBall(1, 1);

            if (isLeftToRight)
            {
                RemoveBall(0, 0);
                RemoveBall(2, 2);
                return;
            }

            RemoveBall(0, 2);
            RemoveBall(2, 0);
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
                RemoveBall(columnIndex, rowIndex);
            }
        }

        private void RemoveBall(int removeAtColumnIndex, int removeAtRowIndex)
        {
            if (Balls[removeAtColumnIndex, removeAtRowIndex] == null)
            {
                return;
            }

            Balls[removeAtColumnIndex, removeAtRowIndex].ReturnToPool();
            Balls[removeAtColumnIndex, removeAtRowIndex] = null;
        }

        private void ShiftBallsInMatrix()
        {
            const int INVALID_INDEX_VALUE = int.MinValue;

            for (int columnIndex = 0; columnIndex < MATRIX_SIZE; columnIndex++)
            {
                int lowestEmptyRowIndex = INVALID_INDEX_VALUE;

                for (int rowIndex = MATRIX_SIZE - 1; rowIndex >= 0; rowIndex--)
                {
                    if (Balls[columnIndex, rowIndex] == null)
                    {
                        if (lowestEmptyRowIndex < rowIndex)
                        {
                            lowestEmptyRowIndex = rowIndex;
                        }

                        continue;
                    }

                    if (lowestEmptyRowIndex != INVALID_INDEX_VALUE)
                    {
                        Balls[columnIndex, lowestEmptyRowIndex] = Balls[columnIndex, rowIndex];
                        Balls[columnIndex, rowIndex] = null;
                        lowestEmptyRowIndex--;
                    }
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