namespace Assets.Logic.Runtime.Containers
{
    using Assets.Logic.Runtime.Balls;
    using Assets.Logic.Runtime.Balls.Data;
    using Assets.Logic.Runtime.Time;

    public class ContainersManager
    {
        private const int MATRIX_SIZE = 3;
        private const int FREE_SLOTS_MAXIMUM_SIZE = MATRIX_SIZE * MATRIX_SIZE;
        private const float CHECK_MATCHES_PERIOD_DURATION_TIME = 0.6f;

        private readonly Ball[,] Balls = new Ball[MATRIX_SIZE, MATRIX_SIZE];
        private readonly ContainerTrigger BallTrigger;
        private readonly TimerEntity DoubleCheckTimer;

        private int _freeSlotsCount = FREE_SLOTS_MAXIMUM_SIZE;

        public ContainersManager(ContainerTrigger ballTrigger)
        {
            BallTrigger = ballTrigger;
            BallTrigger.OnBallEntered += OnBallEntered;

            // Invoke timer only if any diagonal was removed! Check only rows in this case (except for row with index 0)
            DoubleCheckTimer = new TimerEntity(CHECK_MATCHES_PERIOD_DURATION_TIME, onTimeIsUp: PerformDoubleCheck, startTimerOnCreation: false);
        }

        private void AddScore(BallData ballData)
        {
            GameContext.ScoreManager.AddScore(ballData.Score);
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
                    _freeSlotsCount--;
                    Balls[columnIndex, rowIndex] = ball;
                    CheckMatches(columnIndex, rowIndex, ball.BallData);
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

        private void CheckMatches(int checkColumnIndex, int checkRowIndex, BallData ballData)
        {
            int ballId = ballData.Id;
            bool matchColumn = HasColumnMatch(checkColumnIndex, ballId);
            bool matchRow = HasRowMatch(checkRowIndex, ballId);
            HasDiagonalMatches(checkColumnIndex, checkRowIndex, ballId, out bool matchLeftToRightDiagonal, out bool matchRightToLeftDiagonal);

            if (matchColumn)
            {
                ClearColumn(checkColumnIndex);
                AddScore(ballData);
            }

            if (matchRow)
            {
                ClearRow(checkRowIndex);
                AddScore(ballData);
            }

            if (matchLeftToRightDiagonal)
            {
                ClearDiagonal(true);
                AddScore(ballData);
            }

            if (matchRightToLeftDiagonal)
            {
                ClearDiagonal(false);
                AddScore(ballData);
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

            CheckFailState();
        }

        private void CheckFailState()
        {
            if (_freeSlotsCount == 0)
            {
                GameContext.LevelManager.FailLevel();
            }
        }

        private void PerformDoubleCheck()
        {
            const int ROW_INDEX_1 = 1;
            const int ROW_INDEX_2 = 2;
            const int COLUMN_INDEX_DUMMY = 0;

            BallData ballDataRowIndex1 = Balls[COLUMN_INDEX_DUMMY, ROW_INDEX_1]?.BallData;
            BallData ballDataRowIndex2 = Balls[COLUMN_INDEX_DUMMY, ROW_INDEX_2]?.BallData;

            bool hasMatchRowIndex1 = ballDataRowIndex1 != null && HasRowMatch(1, ballDataRowIndex1.Id);
            bool hasMatchRowIndex2 = ballDataRowIndex2 != null && HasRowMatch(2, ballDataRowIndex2.Id);
            bool needToShiftBalls = hasMatchRowIndex1 || hasMatchRowIndex2;

            if (hasMatchRowIndex1)
            {
                ClearRow(ROW_INDEX_1);
                AddScore(ballDataRowIndex1);
            }

            if (hasMatchRowIndex2)
            {
                ClearRow(ROW_INDEX_2);
                AddScore(ballDataRowIndex2);
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

        public void ClearAll()
        {
            for (int columnIndex = 0; columnIndex < MATRIX_SIZE; columnIndex++)
            {
                for (int rowIndex = 0; rowIndex < MATRIX_SIZE; rowIndex++)
                {
                    RemoveBall(columnIndex, rowIndex);
                }
            }
        }

        private void RemoveBall(int removeAtColumnIndex, int removeAtRowIndex, bool useParticleEffect = true)
        {
            if (Balls[removeAtColumnIndex, removeAtRowIndex] == null)
            {
                return;
            }

            _freeSlotsCount++;

            Balls[removeAtColumnIndex, removeAtRowIndex].ReturnToPool(useParticleEffect);
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
            // Magic values, dependent on columns position!

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