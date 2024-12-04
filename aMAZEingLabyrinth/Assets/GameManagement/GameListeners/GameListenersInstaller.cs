using System.Collections.Generic;
using VContainer.Unity;

namespace GameManagement
{
    public sealed class GameListenersInstaller : IInitializable
    {
        private readonly GameListenersManager _listenersManager;

        private readonly IEnumerable<IGameListener> _listeners;

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