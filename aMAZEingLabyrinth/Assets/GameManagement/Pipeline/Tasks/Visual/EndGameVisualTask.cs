using GameCore;
using GameUI;

namespace GamePipeline
{
    public sealed class EndGameVisualTask : Task
    {
        private readonly Player _player;
        private readonly EndGameMenu _endGameMenu;

        public EndGameVisualTask(Player player, EndGameMenu endGameMenu)
        {
            _player = player;
            _endGameMenu = endGameMenu;
        }

        protected override void OnRun()
        {
            _endGameMenu.SetWinner(_player.Type.ToString());
            _endGameMenu.Show();

            Finish();
        }
    }
}