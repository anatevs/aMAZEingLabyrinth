using GameCore;
using UnityEngine;

namespace GamePipeline
{
    public class SetActiveBoardUITask : Task
    {
        private readonly ShiftArrowsService _shiftArrows;
        private readonly CellHighlight _cellHighlight;
        private readonly bool _isActive;

        public SetActiveBoardUITask(ShiftArrowsService shiftArrows,
            CellHighlight cellHighlight,
            bool isActive)
        {
            _shiftArrows = shiftArrows;
            _cellHighlight = cellHighlight;
            _isActive = isActive;
        }

        protected override void OnRun()
        {
            if (_isActive)
            {
                _shiftArrows.EnableAllActiveArrows();
            }
            else
            {
                _shiftArrows.DisableAllArrows();
            }

            _cellHighlight.SetActive(_isActive);

            Finish();
        }
    }
}