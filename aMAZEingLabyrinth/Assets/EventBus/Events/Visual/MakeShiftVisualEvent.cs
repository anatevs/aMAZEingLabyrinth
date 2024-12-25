using DG.Tweening;

namespace EventBusNamespace
{
    public readonly struct MakeShiftVisualEvent : IEvent
    {
        public readonly Sequence ShiftSequence;

        public MakeShiftVisualEvent(Sequence shiftSequence)
        {
            ShiftSequence = shiftSequence;
        }
    }
}