namespace Assets.Logic.Runtime.Common.ObjectPooling
{
    using System;

    public interface IPoolable<T>
    {
        void Initialize(Action<T> returnToPool);

        void OnPopped();

        void OnPushed();
    }
}