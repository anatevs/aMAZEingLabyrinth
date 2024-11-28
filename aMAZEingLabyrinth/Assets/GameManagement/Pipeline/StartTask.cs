using GameCore;

namespace GamePipeline
{
    public sealed class StartTask : Task
    {
        private readonly PlayerSelector _playerSelector;

        private readonly PlayersList _players;

        private readonly ShiftArrowsService _shiftArrowsService;

        private readonly CellHighlight _cellHighlight;

        public StartTask(PlayerSelector playerSelector,
            PlayersList playersList,
            ShiftArrowsService shiftArrowsService,
            CellHighlight cellHighlight)
        {
            _playerSelector = playerSelector;
            _players = playersList;
            _shiftArrowsService = shiftArrowsService;
            _cellHighlight = cellHighlight;
        }

        protected override void OnRun()
        {
            _playerSelector.OnPlayerSelected += PlayerSelect;
        }

        protected override void OnFinished()
        {
            _playerSelector.OnPlayerSelected -= PlayerSelect;
        }

        private void PlayerSelect(PlayerType firstPlayer)
        {
            _players.InitPlayers(firstPlayer);

            _shiftArrowsService.EnableAllActiveArrows();
            _cellHighlight.SetActive(true);

            Finish();
        }
    }
}