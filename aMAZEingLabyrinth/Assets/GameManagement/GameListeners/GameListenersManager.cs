using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public sealed class GameListenersManager : MonoBehaviour
    {
        private readonly List<IGameListener> _gameListeners = new();

        private readonly List<IStartGameListener> _startGameListeners = new();

        private readonly List<IEndGameListener> _endGameListeners = new();

        private readonly List<IAppQuitListener> _appQuitListeners = new();

        public void AddListeners(IEnumerable<IGameListener> listeners)
        {
            foreach (IGameListener listener in listeners)
            {
                AddListener(listener);
            }
        }

        public void AddListener(IGameListener listener)
        {
            _gameListeners.Add(listener);

            if (listener is IStartGameListener startListener)
            {
                _startGameListeners.Add(startListener);
            }

            if (listener is IEndGameListener endGameListener)
            {
                _endGameListeners.Add(endGameListener);
            }

            if (listener is IAppQuitListener quitListener)
            {
                _appQuitListeners.Add(quitListener);
            }
        }

        public void OnStartGame()
        {
            foreach (var listener in _startGameListeners)
            {
                listener.StartGame();
            }
        }

        public void OnEndGame()
        {
            foreach (var listener in _endGameListeners)
            {
                listener.EndGame();
            }
        }

        public void OnApplicationQuit()
        {
            foreach (var listener in _appQuitListeners)
            {
                listener.OnAppQuit();
            }
        }
    }
}