using DG.Tweening;
using GameCore;

namespace EventBusNamespace
{
    public readonly struct MakeShiftVisualEvent : IEvent
    {
        public readonly CellsLabyrinth CellsLabyrinth;

        public readonly Sequence PlayersViewsShift;

        public readonly Sequence PostSequence;

        public MakeShiftVisualEvent(CellsLabyrinth cellsLabyrinth,
            Sequence playersViewsShift, Sequence postSequence)
        {
            CellsLabyrinth = cellsLabyrinth;
            PlayersViewsShift = playersViewsShift;
            PostSequence = postSequence;
        }
    }
}