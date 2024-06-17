namespace Assets.Logic.Runtime.Balls
{
    using Assets.Logic.Runtime.Common.ObjectPooling;
    using Assets.Logic.Runtime.Time;
    using System;
    using UnityEngine;

    public class BallParticleEffect : MonoBehaviour, IPoolable<BallParticleEffect>
    {
        private Action<BallParticleEffect> _onReturnToPool;
        private ParticleSystem _particleSystem;
        private TimerEntity _particleSystemLifetimeTimer;

        public void Initialize(Action<BallParticleEffect> onReturnToPool)
        {
            _particleSystem = GetComponent<ParticleSystem>();
            _particleSystemLifetimeTimer = new TimerEntity(GetTotalParticleSystemDuration(), OnParticleSystemLifetimeTimerIsUp, false);
            _onReturnToPool = onReturnToPool;
        }

        public void OnPopped()
        {
            _particleSystem.Play();
            _particleSystemLifetimeTimer.ResetTimer();
        }

        public void OnPushed()
        {
            _particleSystem.Stop();
        }

        private void OnParticleSystemLifetimeTimerIsUp()
        {
            _onReturnToPool?.Invoke(this);
        }

        private float GetTotalParticleSystemDuration()
        {
            ParticleSystem[] particleSystems = GetComponentsInChildren<ParticleSystem>();
            float duration = 0f;

            foreach (ParticleSystem particleSystem in particleSystems)
            {
                if (particleSystem.main.duration > duration)
                {
                    duration = particleSystem.main.duration;
                }
            }

            return duration;
        }
    }
}