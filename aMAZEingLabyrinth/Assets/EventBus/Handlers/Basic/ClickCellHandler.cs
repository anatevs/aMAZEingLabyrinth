using GameCore;
using GameUI;
using UnityEngine;

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
                player.MoveThroughPath(path);

                var target = player.CurrentTarget;

                foreach (var xy in path)
                {
                    if (_cellsLabyrinth.HasCellReward(xy, target))
                    {
                        Debug.Log($"{player} has {target} on path");

                        EventBus.RaiseEvent(new CheckWinEvent(player));

                        return;
                    }
                }

                EventBus.RaiseEvent(new NextPlayerEvent());
            }
            else
            {
                _menusService.NoPathMenu.SetActive();
                evnt.CellHighlight.SetActive(true);
            }
        }
    }
}