namespace Assets.Logic.Runtime.UI
{
    using UnityEngine;
    using UnityEngine.UI;

    public class ExtendedImage : MonoBehaviour
    {
        private Image _image;
        private RectTransform _rectTransform;

        public Sprite Sprite
        {
            get => Image.sprite;
            set => Image.sprite = value;
        }

        public bool IsRaycastTarget
        {
            get => Image.raycastTarget;
            set
            {
                Image.raycastTarget = value;
            }
        }

        public RectTransform RectTransform
        {
            get
            {
                if (_rectTransform == null && !TryGetComponent(out _rectTransform))
                {
                    Debug.LogError($"Can't find RectTransform in {nameof(ExtendedImage)} class!");
                }

                return _rectTransform;
            }
        }

        private Image Image
        {
            get
            {
                if (_image == null && !TryGetComponent(out _image))
                {
                    Debug.LogError($"Can't find Image in {nameof(ExtendedImage)} class!");
                }

                return _image;
            }
        }

        public virtual void SetActive(bool state)
        {
            gameObject.SetActive(state);
        }

        private protected virtual void OnStart()
        {
            IsRaycastTarget = true;
        }

        private void Start()
        {
            OnStart();
        }

        private protected virtual void OnDestroy()
        {
        }
    }
}