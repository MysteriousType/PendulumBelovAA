namespace Assets.Logic.Runtime.Providers
{
    using System;
    using UnityEngine;
    using UnityEngine.EventSystems;

    public class PointerEventsProvider : MonoBehaviour, IPointerClickHandler
    {
        public event Action<PointerEventData> OnClick;

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            OnClick?.Invoke(eventData);
        }
    }
}