using GameCore;

namespace GamePipeline
{
    public sealed class StartTask : Task
    {
        private readonly PlayerSelector _playerSelector;

        private readonly PlayersList _players;

        private readonly ShiftArrowsService _shiftArrowsService;

        public StartTask(PlayerSelector playerSelector,
            PlayersList playersList,
            ShiftArrowsService shiftArrowsService)
        {
            _playerSelector = playerSelector;
            _players = playersList;
            _shiftArrowsService = shiftArrowsService;
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

            Finish();
        }
    }
}