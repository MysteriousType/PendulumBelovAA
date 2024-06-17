namespace Assets.Logic.Runtime.Time
{
    using System;

    public class TimerEntity
    {
        private readonly Action OnTimeIsUp;
        private readonly float Lifetime;
        private readonly bool AutoReset;

        private float _remainingLifetime;
        private bool _isTimerActive;

        public bool IsTimerActive
        {
            get => _isTimerActive;
            private set
            {
                if (value && !_isTimerActive)
                {
                    _isTimerActive = value;
                    GameContext.TimeManager.OnUpdate += OnUpdate;
                    return;
                }

                if (!value)
                {
                    _isTimerActive = value;
                    GameContext.TimeManager.OnUpdate -= OnUpdate;
                }
            }
        }

        public TimerEntity(float lifetime, Action onTimeIsUp = null, bool startTimerOnCreation = true, bool autoReset = false)
        {
            OnTimeIsUp = onTimeIsUp;
            Lifetime = lifetime;
            IsTimerActive = startTimerOnCreation;
            _remainingLifetime = lifetime;
            AutoReset = autoReset;
        }

        public void ResetTimer(bool resumeTimerOnReset = true)
        {
            _remainingLifetime = Lifetime;

            if (resumeTimerOnReset)
            {
                ResumeTimer();
            }
        }

        public void ResumeTimer()
        {
            IsTimerActive = true;
        }

        public void StopTimer()
        {
            IsTimerActive = false;
        }

        private void OnUpdate(float deltaTime)
        {
            _remainingLifetime -= deltaTime;

            if (_remainingLifetime <= 0f)
            {
                OnTimerTimeExpired();
            }
        }

        private void OnTimerTimeExpired()
        {
            IsTimerActive = false;
            OnTimeIsUp?.Invoke();

            if (AutoReset)
            {
                ResetTimer();
            }
        }
    }
}