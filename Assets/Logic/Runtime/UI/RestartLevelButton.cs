namespace Assets.Logic.Runtime.UI
{
    using Assets.Logic.Runtime.UI.Bases;
    using UnityEngine.EventSystems;

    public class RestartLevelButton : BaseButton
    {
        private protected override void OnButtonClickHandler(PointerEventData pointerEventData)
        {
            GameContext.LevelManager.RestartLevel();
        }
    }
}