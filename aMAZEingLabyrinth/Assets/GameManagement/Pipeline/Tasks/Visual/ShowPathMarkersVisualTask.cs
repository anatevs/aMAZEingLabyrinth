using DG.Tweening;
using GameCore;
using System.Collections.Generic;

namespace GamePipeline
{
    public sealed class ShowPathMarkersVisualTask : Task
    {
        private readonly PathMarkersPool _pathMarkersPool;

        public readonly List<(int x, int y)> _path;

        private readonly Sequence _fadeSequence = DOTween.Sequence().Pause();

        public ShowPathMarkersVisualTask(PathMarkersPool pool,
            List<(int x, int y)> path)
        {
            _pathMarkersPool = pool;
            _path = path;
        }

        protected override async void OnRun()
        {
            foreach (var (x, y) in _path)
            {
                var tween = _pathMarkersPool.Spawn(x, y);

                _fadeSequence.Join(tween);
            }

            await _fadeSequence.Play().AsyncWaitForCompletion();

            Finish();
        }
    }
}