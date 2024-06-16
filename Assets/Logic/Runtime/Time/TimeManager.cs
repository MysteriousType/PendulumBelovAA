namespace Assets.Logic.Runtime.Time
{
    using System;
    using UnityEngine;

    public class TimeManager : MonoBehaviour
    {
        public event Action<float> OnUpdate;

        private void Update()
        {
            OnUpdate?.Invoke(Time.deltaTime);
        }
    }
}