using GameCore;
using UnityEngine;

namespace GamePipeline
{
    public class ActivateBoardUITask : Task
    {
        private readonly ShiftArrowsService _shiftArrows;
        private readonly CellHighlight _cellHighlight;

        public ActivateBoardUITask(ShiftArrowsService shiftArrows,
            CellHighlight cellHighlight)
        {
            _shiftArrows = shiftArrows;
            _cellHighlight = cellHighlight;
        }

        protected override void OnRun()
        {
            _shiftArrows.EnableAllActiveArrows();


            Debug.Log("activate task");
            _cellHighlight.SetActive(true);

            Finish();
        }
    }
}