using GameCore;

namespace GamePipeline
{
    public sealed class TurnTask : Task
    {
        private readonly ShiftArrowsService _shiftArrowsService;

        private readonly CellHighlight _cellHighlight;

        public TurnTask(ShiftArrowsService shiftArrowsService,
            CellHighlight cellHighlight)
        {
            _shiftArrowsService = shiftArrowsService;
            _cellHighlight = cellHighlight;
        }

        protected override void OnRun()
        {
            _shiftArrowsService.EnableAllActiveArrows();
            _cellHighlight.SetActive(true);

            Finish();
        }

        protected override void OnFinished()
        {
        }
    }
}