namespace Assets.Logic.Runtime.Containers
{
    using Assets.Logic.Runtime.Balls;

    public class ContainersManager
    {
        private const int MATRIX_SIZE = 3;
        private readonly Ball[,] Balls = new Ball[MATRIX_SIZE, MATRIX_SIZE];

        private void OnBallLanded(Ball ball, int columnIndex)
        {
            for (int rowIndex = 0; rowIndex < MATRIX_SIZE; rowIndex++)
            {
                if (Balls[columnIndex, rowIndex] != null)
                {
                    Balls[columnIndex, rowIndex] = ball;
                    break;
                }
            }
        }
    }
}