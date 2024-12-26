using GamePipeline;

namespace EventBusNamespace
{
    public class MakeShiftVisualHandler : BaseHandler<MakeShiftVisualEvent>
    {
        private readonly AudioVisualPipeline _visualPipeline;

        public MakeShiftVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline) : base(eventBus)
        {
            _visualPipeline = visualPipeline;
        }

        protected override void RaiseEvent(MakeShiftVisualEvent evnt)
        {
            _visualPipeline.AddTask(new PlayTweenVisualTask(evnt.CellsLabyrinth.PreshiftOldPlayableView()));
            _visualPipeline.AddTask(new MakeShiftVisualTask(evnt.CellsLabyrinth, evnt.PlayersViewsShift));
            _visualPipeline.AddTask(new PlaySequenceVisualTask(evnt.PostSequence));
        }
    }
}