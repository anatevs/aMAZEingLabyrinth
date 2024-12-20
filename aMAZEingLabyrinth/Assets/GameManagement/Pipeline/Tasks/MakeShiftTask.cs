using EventBusNamespace;
using GameCore;

namespace GamePipeline
{
    public class MakeShiftTask : Task
    {
        private readonly EventBus _eventBus;
        private readonly ShiftArrowsController _shiftArrows;

        public MakeShiftTask(EventBus eventBus,
            ShiftArrowsController shiftArrows)
        {
            _eventBus = eventBus;
            _shiftArrows = shiftArrows;
        }

        protected override void OnRun()
        {
            _eventBus.RaiseEvent(new MakeShiftEvent(
                _shiftArrows.ClickedIndex.row,
                _shiftArrows.ClickedIndex.col));

            Finish();
        }
    }
}