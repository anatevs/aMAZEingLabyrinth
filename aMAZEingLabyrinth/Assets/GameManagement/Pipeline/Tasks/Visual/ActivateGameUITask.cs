using GameCore;

namespace GamePipeline
{
    public class ActivateGameUITask : Task
    {
        private readonly ShiftArrowsService _shiftArrows;
        private readonly CellHighlight _cellHighlight;

        public ActivateGameUITask(ShiftArrowsService shiftArrows,
            CellHighlight cellHighlight)
        {
            _shiftArrows = shiftArrows;
            _cellHighlight = cellHighlight;
        }

        protected override void OnRun()
        {
            _shiftArrows.EnableAllActiveArrows();

            _cellHighlight.SetActive(true);

            Finish();
        }
    }
}