using GamePipeline;

namespace EventBusNamespace
{
    public sealed class ShowNoPathVisualHandler : VisualHandler<ShowNoPathVisualEvent>
    {
        public ShowNoPathVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline)
            : base(eventBus, visualPipeline)
        {
        }

        protected override void RaiseEvent(ShowNoPathVisualEvent evnt)
        {
            VisualPipeline.AddTask(new ShowNoPathVisualTask(evnt.NoPathWindow));
        }
    }
}