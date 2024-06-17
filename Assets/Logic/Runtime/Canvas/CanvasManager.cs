namespace Assets.Logic.Runtime.Canvas
{
    using Assets.Logic.Runtime.Common.Extensions;
    using UnityEngine;
    using UnityEngine.UI;

    public class CanvasManager
    {
        private readonly GameObject MainMenuPanel;
        private readonly RectTransform MainMenuTitleTransform;
        private readonly GameObject EndgamePanel;
        private readonly Text EndgameTitleText;

        private int? _titleScaleTweenId = null;

        public CanvasManager()
        {
            Canvas canvas = Object.FindObjectOfType<Canvas>();

            MainMenuPanel = canvas.FindChildByName("MainMenuPanel").gameObject;
            MainMenuTitleTransform = MainMenuPanel.transform.FindComponentInChild<RectTransform>("TitleText");
            EndgamePanel = canvas.FindChildByName("EndgamePanel").gameObject;
            EndgameTitleText = EndgamePanel.transform.FindComponentInChild<Text>("TitleText");
        }

        public void OpenEndgameMenu()
        {
            EndgamePanel.SetActive(true);
            EndgameTitleText.text = $"YOUR SCORE: {GameContext.ScoreManager.Score}";
        }

        public void CloseEndgameMenu()
        {
            EndgamePanel.SetActive(false);
        }

        public void OpenMainMenu()
        {
            MainMenuPanel.SetActive(true);
            EnableMainMenuTitleTweeen();
        }

        public void OpenMainMenuFromLevel()
        {
            OpenMainMenu();
            CloseEndgameMenu();
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