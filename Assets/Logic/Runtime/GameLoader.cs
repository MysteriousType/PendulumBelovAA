namespace Assets.Logic.Runtime
{
    using Assets.Logic.Runtime.Common;

    public class GameLoader : BaseMonoBehaviourSingleton<GameLoader>
    {
        private protected override bool OnSingletonInitialized()
        {
            if (base.OnSingletonInitialized())
            {
                GameContext.Initialize();
                DontDestroyOnLoad(this);
                return true;
            }

            return false;
        }
    }
}