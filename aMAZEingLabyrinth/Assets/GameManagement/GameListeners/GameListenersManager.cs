using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public sealed class GameListenersManager : MonoBehaviour
    {
        private readonly List<IGameListener> _gameListeners = new();

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

            if (listener is IAppQuitListener awakeQuitListener)
            {
                _appQuitListeners.Add(awakeQuitListener);
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