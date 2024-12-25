using DG.Tweening;
using UnityEngine;

namespace GamePipeline
{
    public class MakeShiftVisualTask : Task
    {
        private readonly Sequence _shiftSequence;

        public MakeShiftVisualTask(Sequence shiftSequence)
        {
            _shiftSequence = shiftSequence;
        }

        protected override async void OnRun()
        {
            await _shiftSequence.Play().AsyncWaitForCompletion();
            Finish();
        }
    }
}