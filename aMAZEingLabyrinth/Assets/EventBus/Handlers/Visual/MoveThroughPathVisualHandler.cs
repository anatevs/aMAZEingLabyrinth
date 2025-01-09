using GameCore;
using GamePipeline;

namespace EventBusNamespace
{
    public sealed class MoveThroughPathVisualHandler : VisualHandler<MoveThroughPathEvent>
    {
        private readonly ShiftArrowsService _shiftArrows;
        private readonly CellHighlight _cellHighlight;
        private readonly PathMarkersPool _pathMarkersPool;

        public MoveThroughPathVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline,
            ShiftArrowsService shiftArrows,
            CellHighlight cellHighlight,
            PathMarkersPool pathMarkersPool)
            : base(eventBus, visualPipeline)
        {
            _shiftArrows = shiftArrows;
            _cellHighlight = cellHighlight;
            _pathMarkersPool = pathMarkersPool;
        }

        protected override void RaiseEvent(MoveThroughPathEvent evnt)
        {
            VisualPipeline.AddTask(new ShowPathMarkersVisualTask(
                _pathMarkersPool, evnt.Path));

            VisualPipeline.AddTask(new MoveThroughPathVisualTask
                (evnt.Player, evnt.Path));

            VisualPipeline.AddTask(new HidePathMarkersVisualTask(
                _pathMarkersPool));

            VisualPipeline.AddTask(new SetActiveBoardUITask(
                _shiftArrows, _cellHighlight, true));
        }
    }
}