using DG.Tweening;
using GameCore;

namespace EventBusNamespace
{
    public readonly struct MakeShiftVisualEvent : IEvent
    {
        public readonly Sequence ShiftSequence;

        public readonly Sequence PostSequence;
        public MakeShiftVisualEvent(Sequence shiftSequence, Sequence postSequence)
        {
            ShiftSequence = shiftSequence;
            PostSequence = postSequence;
        }
    }
}