using GameCore;

namespace GamePipeline
{
    public sealed class TurnTask : Task
    {
        private readonly CellHighlight _cellHighlight;

        public TurnTask(ShiftArrowsService shiftArrowsService,
            CellHighlight cellHighlight)
        {
            _cellHighlight = cellHighlight;
        }

        protected override void OnRun()
        {
            _cellHighlight.SetActive(true);

            Finish();
        }

        protected override void OnFinished()
        {
        }
    }
}