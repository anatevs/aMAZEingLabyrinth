using EventBusNamespace;
using GameCore;
using GameUI;
using System.Collections;
using UnityEngine;

namespace EventBusNamespace
{
    public class ClickCellHandler : BaseHandler<ClickCellEvent>
    {
        private readonly PlayersList _playersList;
        private readonly CellsLabyrinth _cellsLabyrinth;
        private readonly MenusService _menusService;

        public ClickCellHandler(EventBus eventBus,
            PlayersList playersList,
            CellsLabyrinth cellsLabyrinth,
            MenusService menusService) : base(eventBus)
        {
            _playersList = playersList;
            _cellsLabyrinth = cellsLabyrinth;
            _menusService = menusService;
        }

        protected override void RaiseEvent(ClickCellEvent evnt)
        {
            var startXY = _playersList.CurrentPlayer.Coordinate;
            var endXY = evnt.CellHighlight.CurrentPosition;

            var result = _cellsLabyrinth.FindPath(startXY, endXY, out var path);

            if (result)
            {
                _playersList.CurrentPlayer.MoveThroughPath(path);

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