using System.Collections.Generic;
using VContainer.Unity;

namespace GameManagement
{
    public class GameListenersInstaller : IInitializable
    {
        private GameListenersManager _listenersManager;

        private IEnumerable<IGameListener> _listeners;

        public GameListenersInstaller(GameListenersManager gameListenersManager, IEnumerable<IGameListener> gameListeners)
        {
            _listenersManager = gameListenersManager;
            _listeners = gameListeners;
        }

        public void Initialize()
        {
            _listenersManager.AddListeners(_listeners);
        }
    }
}