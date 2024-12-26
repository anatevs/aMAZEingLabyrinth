using DG.Tweening;

namespace GamePipeline
{
    public sealed class PlayTweenVisualTask : Task
    {
        private readonly Tween _tween;

        public PlayTweenVisualTask(Tween tween)
        {
            _tween = tween;
        }

        protected override async void OnRun()
        {
            await _tween.Play().AsyncWaitForCompletion();

            Finish();
        }
    }
}