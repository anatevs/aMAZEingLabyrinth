using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public class GameListenersManager : MonoBehaviour
    {
        private readonly List<IGameListener> gameListeners = new List<IGameListener>();

        private readonly List<IAppQuitListener> appQuitListeners = new List<IAppQuitListener>();

        public void AddListeners(IEnumerable<IGameListener> listeners)
        {
            foreach (IGameListener listener in listeners)
            {
                AddListener(listener);
            }
        }

        public void AddListener(IGameListener listener)
        {
            gameListeners.Add(listener);

            if (listener is IAppQuitListener awakeQuitListener)
            {
                appQuitListeners.Add(awakeQuitListener);
            }
        }

        public void OnApplicationQuit()
        {
            foreach (var listener in appQuitListeners)
            {
                listener.OnAppQuit();
            }
        }
    }
}