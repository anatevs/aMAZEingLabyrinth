using GamePipeline;

namespace EventBusNamespace
{
    public sealed class MoveThroughPathVisualHandler : BaseHandler<MoveThroughPathEvent>
    {
        private readonly AudioVisualPipeline _visualPipeline;

        public MoveThroughPathVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline) : base(eventBus)
        {
            _visualPipeline = visualPipeline;
        }

        protected override void RaiseEvent(MoveThroughPathEvent evnt)
        {
            _visualPipeline.AddTask(new MoveThroughPathVisualTask
                (evnt.Player, evnt.Path));
        }
    }
}