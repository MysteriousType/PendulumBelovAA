namespace Assets.Logic.Runtime.Common.Extensions
{
    public static class LeanTweenExtensions
    {
        public static void TryResume(int? tweenId)
        {
            if (tweenId.HasValue)
            {
                LTDescr descr = LeanTween.descr(tweenId.Value);
                descr?.resume();
            }
        }

        public static void TryPause(int? tweenId)
        {
            if (tweenId.HasValue)
            {
                LTDescr descr = LeanTween.descr(tweenId.Value);
                descr?.pause();
            }
        }

        public static void TryCancel(int? tweenId)
        {
            if (tweenId.HasValue)
            {
                LeanTween.cancel(tweenId.Value);
            }
        }
    }
}