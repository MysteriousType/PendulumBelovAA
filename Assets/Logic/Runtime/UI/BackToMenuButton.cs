namespace Assets.Logic.Runtime.UI
{
    using Assets.Logic.Runtime.UI.Bases;
    using UnityEngine.EventSystems;

    public class BackToMenuButton : BaseButton
    {
        private protected override void OnButtonClickHandler(PointerEventData pointerEventData)
        {
            GameContext.CanvasManager.OpenMainMenuFromLevel();
        }
    }
}