namespace Assets.Logic.Runtime.UI.Bases
{
    using Assets.Logic.Runtime.Providers;
    using UnityEngine.EventSystems;

    public abstract class BaseButton : ExtendedImage
    {
        private PointerEventsProvider _pointerEventsProvider;

        private PointerEventsProvider PointerEventsProvider
        {
            get
            {
                if (_pointerEventsProvider == null)
                {
                    _pointerEventsProvider = gameObject.AddComponent<PointerEventsProvider>();
                }

                return _pointerEventsProvider;
            }
        }

        private protected override void OnStart()
        {
            base.OnStart();
            PointerEventsProvider.OnClick += OnButtonClickHandler;
        }

        private protected abstract void OnButtonClickHandler(PointerEventData pointerEventData);
    }
}