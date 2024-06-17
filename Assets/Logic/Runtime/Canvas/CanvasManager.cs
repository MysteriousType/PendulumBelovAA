namespace Assets.Logic.Runtime.Canvas
{
    using Assets.Logic.Runtime.Common.Extensions;
    using UnityEngine;

    public class CanvasManager
    {
        private readonly GameObject MainMenuPanel;
        private readonly RectTransform MainMenuTitleTransform;

        private int? _titleScaleTweenId = null;

        public CanvasManager()
        {
            MainMenuPanel = GameObject.Find("MainMenuPanel");
            MainMenuTitleTransform = MainMenuPanel.transform.FindComponentInChild<RectTransform>("TitleText");
        }

        public void OpenMainMenu()
        {
            MainMenuPanel.SetActive(true);
            EnableMainMenuTitleTweeen();
        }

        public void CloseMainMenu()
        {
            MainMenuPanel.SetActive(false);
            DisableMainMenuTitleTweeen();
        }

        private void EnableMainMenuTitleTweeen()
        {
            if (_titleScaleTweenId == null)
            {
                StartMainMenuTitleTweeen();
                return;
            }

            LeanTweenExtensions.TryResume(_titleScaleTweenId);
        }

        private void DisableMainMenuTitleTweeen()
        {
            LeanTweenExtensions.TryPause(_titleScaleTweenId);
        }

        private void StartMainMenuTitleTweeen()
        {
            const float TWEEN_TIME = 1f;
            const float START_SCALE_MULTIPLIER = 0.66f;
            const float END_SCALE_MULTIPLIER = 1f;

            LeanTweenExtensions.TryCancel(_titleScaleTweenId);
            _titleScaleTweenId = LeanTween
                .value(START_SCALE_MULTIPLIER, END_SCALE_MULTIPLIER, TWEEN_TIME)
                .setEase(LeanTweenType.linear)
                .setOnUpdate(OnTweenUpdate)
                .setLoopPingPong()
                .id;

            void OnTweenUpdate(float value)
            {
                MainMenuTitleTransform.localScale = Vector3.one * value;
            }
        }
    }
}