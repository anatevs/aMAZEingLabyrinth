using GameUI;
using System;
using UnityEngine;
using VContainer.Unity;

namespace GameManagement
{
    public class EndGameManager : IInitializable, IDisposable
    {
        private readonly MenusService _menusService;

        private readonly GameListenersManager _gameListenersManager;

        public EndGameManager(MenusService menusService, GameListenersManager gameListenersManager)
        {
            _menusService = menusService;
            _gameListenersManager = gameListenersManager;
        }

        void IInitializable.Initialize()
        {
            _menusService.EndGame.OnExitClicked += QuitGame;
        }

        void IDisposable.Dispose()
        {
            _menusService.EndGame.OnExitClicked -= QuitGame;
        }

        private void QuitGame()
        {
            Application.Quit();
        }
    }
}