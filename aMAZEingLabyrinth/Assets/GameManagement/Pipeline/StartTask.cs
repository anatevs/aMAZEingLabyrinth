using GameCore;
using GameUI;
using UnityEngine;

namespace GamePipeline
{
    public sealed class StartTask : Task
    {
        private readonly MenusService _menusService;

        private readonly PlayersList _players;

        private readonly ShiftArrowsService _shiftArrowsService;

        private readonly CellHighlight _cellHighlight;

        public StartTask(MenusService menuWindowsService,
            PlayersList playersList,
            ShiftArrowsService shiftArrowsService,
            CellHighlight cellHighlight)
        {
            _menusService = menuWindowsService;
            _players = playersList;
            _shiftArrowsService = shiftArrowsService;
            _cellHighlight = cellHighlight;
        }

        protected override void OnRun()
        {
            _menusService.PlayerSelector.OnPlayerSelected += SelectFirstPlayer;
        }

        protected override void OnFinished()
        {
            _menusService.PlayerSelector.OnPlayerSelected -= SelectFirstPlayer;
        }

        private void SelectFirstPlayer(PlayerType firstPlayer)
        {
            _players.InitPlayers(firstPlayer);

            _shiftArrowsService.EnableAllActiveArrows();
            _cellHighlight.SetActive(true);

            Finish();
        }
    }
}