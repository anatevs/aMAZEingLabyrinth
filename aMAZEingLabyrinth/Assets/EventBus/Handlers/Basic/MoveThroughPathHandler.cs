using GameCore;

namespace EventBusNamespace
{
    public sealed class MoveThroughPathHandler : BaseHandler<MoveThroughPathEvent>
    {
        private readonly CellsLabyrinth _cellsLabyrinth;

        public MoveThroughPathHandler(EventBus eventBus,
            CellsLabyrinth cellsLabyrinth)
            : base(eventBus)
        {
            _cellsLabyrinth = cellsLabyrinth;
        }

        protected override void RaiseEvent(MoveThroughPathEvent evnt)
        {
            var player = evnt.Player;
            var path = evnt.Path;


            player.MoveThroughPath(path);

            var target = player.CurrentTarget;

            foreach (var xy in path)
            {
                if (_cellsLabyrinth.HasCellReward(xy, target))
                {
                    EventBus.RaiseEvent(new CheckWinEvent(player));

                    return;
                }
            }

            EventBus.RaiseEvent(new NextPlayerEvent());
        }
    }
}