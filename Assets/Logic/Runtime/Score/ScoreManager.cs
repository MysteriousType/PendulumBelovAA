namespace Assets.Logic.Runtime.Score
{
    public class ScoreManager
    {
        private const int SCORE_MINIMUM_VALUE = 0;

        public int Score { get; private set; }

        public ScoreManager()
        {
            Score = SCORE_MINIMUM_VALUE;
        }

        public void AddScore(int score)
        {
            Score += score;
        }

        public void ResetScore()
        {
            Score = SCORE_MINIMUM_VALUE;
        }
    }
}