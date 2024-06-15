namespace Assets.Logic.Runtime.Common
{
    using UnityEngine;

    public abstract class BaseMonoBehaviourSingleton<T> : MonoBehaviour where T : BaseMonoBehaviourSingleton<T>
    {
        public static T Instance { get; private set; }

        private protected virtual void OnStart()
        {
        }

        private void Awake()
        {
            OnSingletonInitialized();
        }

        private void Start()
        {
            OnStart();
        }

        private protected virtual bool OnSingletonInitialized()
        {
            if (Instance == null || Instance == this)
            {
                Instance = this as T;
                return true;
            }

            Destroy(gameObject);
            return false;
        }
    }
}
