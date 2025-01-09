using EventBusNamespace;
using GameCore;

namespace GamePipeline
{
    public sealed class TurnTask : Task
    {
        private readonly CellHighlight _cellHighlight;
        private readonly EventBus _eventBus;

        public TurnTask(
            CellHighlight cellHighlight,
            EventBus eventBus)
        {
            _cellHighlight = cellHighlight;
            _eventBus = eventBus;
        }

        protected override void OnRun()
        {
            _eventBus.RaiseEvent(new ClickCellEvent(_cellHighlight));

            Finish();
        }

        protected override void OnFinished()
        {
        }
    }
}