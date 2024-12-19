using GameCore;
using GamePipeline;

namespace EventBusNamespace
{
    public sealed class MoveThroughPathVisualHandler : BaseHandler<MoveThroughPathEvent>
    {
        private readonly AudioVisualPipeline _visualPipeline;
        private readonly CellHighlight _cellHighlight;

        public MoveThroughPathVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline,
            CellHighlight cellHighlight) : base(eventBus)
        {
            _visualPipeline = visualPipeline;
            _cellHighlight = cellHighlight;
        }

        protected override void RaiseEvent(MoveThroughPathEvent evnt)
        {
            _visualPipeline.AddTask(new MoveThroughPathVisualTask
                (evnt.Player, evnt.Path, _cellHighlight));
        }
    }
}