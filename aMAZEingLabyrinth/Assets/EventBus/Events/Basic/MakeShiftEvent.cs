namespace EventBusNamespace
{
    public readonly struct MakeShiftEvent : IEvent
    {
        public readonly int Row;

        public readonly int Col;


        public MakeShiftEvent((int row, int col) index)
        {
            Row = index.row;
            Col = index.col;
        }
    }
}