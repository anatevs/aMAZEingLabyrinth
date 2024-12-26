using GameCore;
using GameUI;

namespace EventBusNamespace
{
    public sealed class ClickCellHandler : BaseHandler<ClickCellEvent>
    {
        private readonly PlayersList _playersList;
        private readonly CellsLabyrinth _cellsLabyrinth;
        private readonly MenusService _menusService;
        private readonly ShiftArrowsService _shiftsArrows;

        public ClickCellHandler(EventBus eventBus,
            PlayersList playersList,
            CellsLabyrinth cellsLabyrinth,
            MenusService menusService,
            ShiftArrowsService shiftsArrowsService) : base(eventBus)
        {
            _playersList = playersList;
            _cellsLabyrinth = cellsLabyrinth;
            _menusService = menusService;
            _shiftsArrows = shiftsArrowsService;
        }

        protected override void RaiseEvent(ClickCellEvent evnt)
        {
            evnt.CellHighlight.SetActive(false);
            _shiftsArrows.DisableAllArrows();

            var player = _playersList.CurrentPlayer;

            var startXY = player.Coordinate;
            var endXY = evnt.CellHighlight.CurrentPosition;

            var result = _cellsLabyrinth.FindPath(startXY, endXY, out var path);

            if (result)
            {
                EventBus.RaiseEvent(new MoveThroughPathEvent(player, path));
            }
            else
            {
                EventBus.RaiseEvent(new ShowNoPathVisualEvent(_menusService.NoPathMenu));
                evnt.CellHighlight.SetActive(true);
            }
        }
    }
}