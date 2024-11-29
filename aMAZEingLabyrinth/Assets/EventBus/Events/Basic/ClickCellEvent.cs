using GameCore;

namespace EventBusNamespace
{
    public readonly struct ClickCellEvent : IEvent
    {
        public readonly CellHighlight CellHighlight;

        public ClickCellEvent(CellHighlight initHighlight)
        {
            CellHighlight = initHighlight;
        }
    }
}