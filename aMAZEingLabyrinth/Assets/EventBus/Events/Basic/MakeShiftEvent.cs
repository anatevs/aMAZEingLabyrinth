using GameCore;

namespace EventBusNamespace
{
    public readonly struct MakeShiftEvent : IEvent
    {
        public readonly ShiftArrowsService ShiftArrowsService;
        public readonly CellsLabyrinth CellsLabyrinth;
        public readonly int Row;
        public readonly int Col;

        public MakeShiftEvent(ShiftArrowsService initArrowsService,
            CellsLabyrinth initLabyrinth,
            int initRow, int initCol)
        {
            ShiftArrowsService = initArrowsService;
            CellsLabyrinth = initLabyrinth;
            Row = initRow;
            Col = initCol;
        }
    }
}