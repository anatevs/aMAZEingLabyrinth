using GameCore;

namespace GamePipeline
{
    public sealed class StartTask : Task
    {
        private readonly PlayerSelector _playerSelector;

        private readonly PlayersList _players;

        public StartTask(PlayerSelector playerSelector, PlayersList playersList)
        {
            _playerSelector = playerSelector;
            _players = playersList;
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

            Finish();
        }
    }
}