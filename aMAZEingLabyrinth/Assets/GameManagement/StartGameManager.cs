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

        private readonly CellHighlight _cellHighlight;

        private readonly PlayersList _players;

        private readonly CellsLabyrinth _cellsLabyrinth;

        private readonly ShiftArrowsService _shiftArrowsService;

        private readonly SaveLoadManager _saveLoadManager;

        private readonly TurnPipeline _turnPipeline;

        public StartGameManager(MenusService menuWindowsService,
            CellHighlight cellHighlight,
            PlayersList playersList,
            TurnPipeline turnPipeline,
            CellsLabyrinth cellsLabyrinth,
            ShiftArrowsService shiftArrowsService,
            SaveLoadManager saveLoadManager)
        {
            _menusService = menuWindowsService;
            _cellHighlight = cellHighlight;
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

            _menusService.InGameMenu.OnNewGameClicked += SelectNewGame;

            _menusService.ChoosingPlayer.OnPlayerToggled += _players.SetPlayerToList;

            _players.OnListChanged += _menusService.PlayerSelector.InitDropdown;

            _menusService.PlayerSelector.OnPlayerSelected += SelectFirstPlayer;

            var isDataInRepository = _saveLoadManager.IsDataInRepository;

            _menusService.StartGame.SetLoadButtonActive(isDataInRepository);
        }

        void IDisposable.Dispose()
        {
            _menusService.StartGame.OnLoadGameClicked -= SelectLoadGame;

            _menusService.StartGame.OnNewGameClicked -= SelectNewGame;

            _menusService.EndGame.OnNewGameClicked -= SelectNewGame;

            _menusService.InGameMenu.OnNewGameClicked -= SelectNewGame;

            _menusService.ChoosingPlayer.OnPlayerToggled -= _players.SetPlayerToList;

            _players.OnListChanged -= _menusService.PlayerSelector.InitDropdown;

            _menusService.PlayerSelector.OnPlayerSelected -= SelectFirstPlayer;
        }

        private void SelectNewGame()
        {
            _saveLoadManager.LoadNewGame();

            _cellHighlight.SetActive(false);

            _menusService.PlayerSelector.Show();
        }

        //make Init of active players selection



        private void SelectFirstPlayer(int firstPlayerIndex)
        {
            _players.InitPlayers(firstPlayerIndex);

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