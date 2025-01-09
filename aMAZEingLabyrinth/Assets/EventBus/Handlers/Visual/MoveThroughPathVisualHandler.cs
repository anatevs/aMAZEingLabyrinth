using GameCore;
using GamePipeline;

namespace EventBusNamespace
{
    public sealed class MoveThroughPathVisualHandler : BaseHandler<MoveThroughPathEvent>
    {
        private readonly AudioVisualPipeline _visualPipeline;
        private readonly ShiftArrowsService _shiftArrows;
        private readonly CellHighlight _cellHighlight;
        private readonly PathMarkersPool _pathMarkersPool;

        public MoveThroughPathVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline,
            ShiftArrowsService shiftArrows,
            CellHighlight cellHighlight,
            PathMarkersPool pathMarkersPool) : base(eventBus)
        {
            _visualPipeline = visualPipeline;
            _shiftArrows = shiftArrows;
            _cellHighlight = cellHighlight;
            _pathMarkersPool = pathMarkersPool;
        }

        protected override void RaiseEvent(MoveThroughPathEvent evnt)
        {
            _visualPipeline.AddTask(new ShowPathMarkersVisualTask(
                _pathMarkersPool, evnt.Path));

            _visualPipeline.AddTask(new MoveThroughPathVisualTask
                (evnt.Player, evnt.Path));

            _visualPipeline.AddTask(new HidePathMarkersVisualTask(
                _pathMarkersPool));

            _visualPipeline.AddTask(new SetActiveBoardUITask(
                _shiftArrows, _cellHighlight, true));
        }
    }
}