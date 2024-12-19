using EventBusNamespace;
using GameCore;
using GameUI;

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

            _cellHighlight.SetActive(false);

            Finish();
        }

        protected override void OnFinished()
        {
        }
    }
}