namespace Assets.Logic.Runtime.Common.ObjectPooling
{
    using System;
    using System.Collections.Generic;

    public class ObjectPool<T> where T : IPoolable<T>
    {
        private readonly Func<T> CreateObject;
        private readonly Stack<T> PooledObjects;

        public ObjectPool(Func<T> createObject, int poolInitialCapacity = 0)
        {
            CreateObject = createObject;
            PooledObjects = new Stack<T>(poolInitialCapacity);
        }

        public T Pop()
        {
            T objectToPop;

            if (PooledObjects.Count == 0)
            {
                objectToPop = CreateObject.Invoke();
                objectToPop.Initialize(Push);
            }
            else
            {
                objectToPop = PooledObjects.Pop();
            }

            objectToPop.OnPopped();
            return objectToPop;
        }

        public void Push(T objectToPush)
        {
            PooledObjects.Push(objectToPush);
            objectToPush.OnPushed();
        }
    }
}