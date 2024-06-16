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
            // Check column for match
            for (int rowIndex = 0; rowIndex < MATRIX_SIZE; rowIndex++)
            {
                Ball ball = Balls[checkColumnIndex, rowIndex];

                if (ball == null || ball.BallData.Id != ballId)
                {
                    break;
                }
            }

            // Check row for match
            for (int columnIndex = 0; columnIndex < MATRIX_SIZE; columnIndex++)
            {
                Ball ball = Balls[columnIndex, checkRowIndex];

                if (ball == null || ball.BallData.Id != ballId)
                {
                    break;
                }
            }

            // Check diagonal  for match
            for (int index = 0; index < MATRIX_SIZE; index++)
            {
                Ball ball = Balls[index, index];

                if (ball == null || ball.BallData.Id != ballId)
                {
                    break;
                }
            }
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