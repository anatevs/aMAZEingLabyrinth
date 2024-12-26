using DG.Tweening;
using GameCore;

namespace GamePipeline
{
    public sealed class MakeShiftVisualTask : Task
    {
        private readonly CellsLabyrinth _cellsLabyrinth;

        private readonly Sequence _playersViewsShift;

        public MakeShiftVisualTask(CellsLabyrinth cellsLabyrinth,
            Sequence playersViewsShift)
        {
            _cellsLabyrinth = cellsLabyrinth;
            _playersViewsShift = playersViewsShift;
        }

        protected override async void OnRun()
        {
            var sequence = _cellsLabyrinth.PrepareShiftViews();

            sequence.Join(_playersViewsShift);

            await sequence.Play().AsyncWaitForCompletion();

            Finish();
        }
    }
}