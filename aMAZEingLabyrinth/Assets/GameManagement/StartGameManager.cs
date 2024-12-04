using GamePipeline;
using GameUI;
using System;
using VContainer.Unity;

namespace GameCore
{
    public class StartGameManager : IInitializable, IDisposable
    {
        private readonly MenusService _menusService;

        private readonly PlayersList _players;

        private readonly TurnPipeline _turnPipeline;

        public StartGameManager(MenusService menuWindowsService,
            PlayersList playersList,
            TurnPipeline turnPipeline)
        {
            _menusService = menuWindowsService;
            _players = playersList;
            _turnPipeline = turnPipeline;
        }

        void IInitializable.Initialize()
        {
            _menusService.StartGame.OnLoadGameClicked += SelectLoadGame;

            _menusService.PlayerSelector.OnPlayerSelected += SelectFirstPlayer;
        }

        void IDisposable.Dispose()
        {
            _menusService.StartGame.OnLoadGameClicked -= SelectLoadGame;

            _menusService.PlayerSelector.OnPlayerSelected -= SelectFirstPlayer;
        }

        private void SelectFirstPlayer(PlayerType firstPlayer)
        {
            _players.InitPlayers(firstPlayer);

            _turnPipeline.Run();
        }

        private void SelectLoadGame()
        {
            _players.InitPlayers();

            _turnPipeline.Run();
        }
    }
}