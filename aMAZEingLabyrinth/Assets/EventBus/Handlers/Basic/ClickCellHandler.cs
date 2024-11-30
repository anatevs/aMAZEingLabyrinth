using GameCore;
using GameUI;
using UnityEngine;

namespace EventBusNamespace
{
    public class ClickCellHandler : BaseHandler<ClickCellEvent>
    {
        private readonly PlayersList _playersList;
        private readonly CellsLabyrinth _cellsLabyrinth;
        private readonly MenusService _menusService;
        private readonly CellHighlight _cellHighlight;

        public ClickCellHandler(EventBus eventBus,
            PlayersList playersList,
            CellsLabyrinth cellsLabyrinth,
            MenusService menusService,
            CellHighlight cellHighlight) : base(eventBus)
        {
            _playersList = playersList;
            _cellsLabyrinth = cellsLabyrinth;
            _menusService = menusService;
            _cellHighlight = cellHighlight;
        }

        protected override void RaiseEvent(ClickCellEvent evnt)
        {
            _cellHighlight.SetActive(false);

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

                        player.ReleaseReward();

                        break;
                    }
                }

                //raise event of remain cards check?

                //handle reward if exist

                //next player
            }
            else
            {
                _menusService.NoPathMenu.SetActive();
            }
        }
    }
}