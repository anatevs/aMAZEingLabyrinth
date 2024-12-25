using GamePipeline;

namespace EventBusNamespace
{
    public sealed class ShowNoPathVisualHandler : BaseHandler<ShowNoPathVisualEvent>
    {
        private readonly AudioVisualPipeline _visualPipeline;

        public ShowNoPathVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline) : base(eventBus)
        {
            _visualPipeline = visualPipeline;
        }

        protected override void RaiseEvent(ShowNoPathVisualEvent evnt)
        {
            _visualPipeline.AddTask(new ShowNoPathVisualTask(evnt.NoPathWindow));
        }
    }
}