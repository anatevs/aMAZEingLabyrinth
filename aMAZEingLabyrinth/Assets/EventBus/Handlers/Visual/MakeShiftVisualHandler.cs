using GamePipeline;

namespace EventBusNamespace
{
    public class MakeShiftVisualHandler : VisualHandler<MakeShiftVisualEvent>
    {
        public MakeShiftVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline)
            : base(eventBus, visualPipeline)
        {
        }

        protected override void RaiseEvent(MakeShiftVisualEvent evnt)
        {
            VisualPipeline.AddTask(new PlayTweenVisualTask(evnt.CellsLabyrinth.PreshiftOldPlayableView()));
            VisualPipeline.AddTask(new MakeShiftVisualTask(evnt.CellsLabyrinth, evnt.PlayersViewsShift));
            VisualPipeline.AddTask(new PlaySequenceVisualTask(evnt.PostSequence));
        }
    }
}