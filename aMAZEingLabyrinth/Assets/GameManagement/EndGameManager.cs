using GameUI;
using System;
using UnityEditor;
using UnityEngine;
using VContainer.Unity;

namespace GameManagement
{
    public class EndGameManager : IInitializable, IDisposable
    {
        private readonly MenusService _menusService;

        public EndGameManager(MenusService menusService)
        {
            _menusService = menusService;
        }

        void IInitializable.Initialize()
        {
            _menusService.EndGame.OnExitClicked += QuitGame;

            _menusService.InGameMenu.OnExitClicked += QuitGame;
        }

        void IDisposable.Dispose()
        {
            _menusService.EndGame.OnExitClicked -= QuitGame;

            _menusService.InGameMenu.OnExitClicked -= QuitGame;
        }

        private void QuitGame()
        {
            Application.Quit();

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#endif

        }
    }
}