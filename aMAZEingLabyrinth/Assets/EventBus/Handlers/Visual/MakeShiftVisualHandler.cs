using GameCore;
using GamePipeline;

namespace EventBusNamespace
{
    public class MakeShiftVisualHandler : BaseHandler<MakeShiftEvent>
    {
        private readonly CellsLabyrinth _cellsLabyrinth;
        private readonly PlayersList _players;
        private readonly AudioVisualPipeline _visualPipeline;

        public MakeShiftVisualHandler(EventBus eventBus,
            CellsLabyrinth cellsLabyrinth,
            PlayersList players,
            AudioVisualPipeline visualPipeline) : base(eventBus)
        {
            _cellsLabyrinth = cellsLabyrinth;
            _players = players;
            _visualPipeline = visualPipeline;
        }

        protected override void RaiseEvent(MakeShiftEvent evnt)
        {
            //_visualPipeline.AddTask(new MakeShiftVisualTask(
            //    _cellsLabyrinth, _players,
            //    (evnt.Row, evnt.Col)));
        }
    }
}