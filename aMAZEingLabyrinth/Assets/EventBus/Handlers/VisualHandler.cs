using GamePipeline;

namespace EventBusNamespace
{
    public abstract class VisualHandler<T> : BaseHandler<T>
    {
        public AudioVisualPipeline VisualPipeline => _visualPipeline;

        private readonly AudioVisualPipeline _visualPipeline;

        protected VisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline) : base(eventBus)
        {
            _visualPipeline = visualPipeline;
        }
    }
}