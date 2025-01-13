using DG.Tweening;
using UnityEngine;

namespace GamePipeline
{
    public sealed class PlaySequenceVisualTask : Task
    {
        private readonly Sequence _sequence;

        public PlaySequenceVisualTask(Sequence sequence)
        {
            _sequence = sequence;
        }

        protected override async void OnRun()
        {
            await _sequence.Play().AsyncWaitForCompletion();
            Finish();
        }
    }
}