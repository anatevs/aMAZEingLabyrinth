using GamePipeline;
using GameUI;
using SaveLoadNamespace;
using System;
using VContainer.Unity;
using GameCore;

namespace GameManagement
{
    public class StartGameManager : IInitializable, IDisposable
    {
        private readonly MenusService _menusService;

        private readonly PlayersList _players;

        private readonly CellsLabyrinth _cellsLabyrinth;

        private readonly ShiftArrowsService _shiftArrowsService;

        private readonly SaveLoadManager _saveLoadManager;

        private readonly TurnPipeline _turnPipeline;

        public StartGameManager(MenusService menuWindowsService,
            PlayersList playersList,
            TurnPipeline turnPipeline,
            CellsLabyrinth cellsLabyrinth,
            ShiftArrowsService shiftArrowsService,
            SaveLoadManager saveLoadManager)
        {
            _menusService = menuWindowsService;
            _players = playersList;
            _turnPipeline = turnPipeline;
            _cellsLabyrinth = cellsLabyrinth;
            _shiftArrowsService = shiftArrowsService;
            _saveLoadManager = saveLoadManager;
        }

        void IInitializable.Initialize()
        {
            _menusService.StartGame.OnLoadGameClicked += SelectLoadGame;

            _menusService.StartGame.OnNewGameClicked += SelectNewGame;

            _menusService.EndGame.OnNewGameClicked += SelectNewGame;

            _menusService.PlayerSelector.OnPlayerSelected += SelectFirstPlayer;


            var isDataInRepository = _saveLoadManager.IsDataInRepository;

            _menusService.StartGame.SetLoadButtonActive(isDataInRepository);
        }

        void IDisposable.Dispose()
        {
            _menusService.StartGame.OnLoadGameClicked -= SelectLoadGame;

            _menusService.StartGame.OnNewGameClicked -= SelectNewGame;

            _menusService.EndGame.OnNewGameClicked -= SelectNewGame;

            _menusService.PlayerSelector.OnPlayerSelected -= SelectFirstPlayer;
        }

        private void SelectNewGame()
        {
            _saveLoadManager.LoadNewGame();

            _menusService.PlayerSelector.Show();
        }

        private void SelectFirstPlayer(PlayerType firstPlayer)
        {
            _players.InitPlayers(firstPlayer);

            _cellsLabyrinth.InitMovableCells();

            _shiftArrowsService.InitArrows();

            _turnPipeline.Run();
        }

        private void SelectLoadGame()
        {
            _players.InitPlayers();

            _cellsLabyrinth.InitMovableCells();

            _shiftArrowsService.InitArrows();

            _turnPipeline.Run();
        }
    }
}