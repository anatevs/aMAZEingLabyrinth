using GameCore;

namespace EventBusNamespace
{
    public readonly struct MakeShiftEvent : IEvent
    {
        public readonly int Row;
        public readonly int Col;

        public MakeShiftEvent(int initRow, int initCol)
        {
            Row = initRow;
            Col = initCol;
        }
    }
}