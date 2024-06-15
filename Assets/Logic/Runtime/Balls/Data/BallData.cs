namespace Assets.Logic.Runtime.Balls.Data
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "BallData", menuName = "ScriptableObjects/Balls/BallData")]
    public class BallData : ScriptableObject
    {
        [Header("Ball Data")]
        [SerializeField]
        private int _id;

        [SerializeField]
        private int _score;

        public int Id => _id;

        public int Score => _score;
    }
}