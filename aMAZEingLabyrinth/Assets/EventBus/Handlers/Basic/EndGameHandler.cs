using GameManagement;

namespace EventBusNamespace
{
    public sealed class EndGameHandler : BaseHandler<EndGameEvent>
    {
        private readonly GameListenersManager _gameListenersManager;

        public EndGameHandler(EventBus eventBus,
            GameListenersManager gameListenersManager) : base(eventBus)
        {
            _gameListenersManager = gameListenersManager;
        }

        protected override void RaiseEvent(EndGameEvent evnt)
        {
            _gameListenersManager.OnEndGame();
        }
    }
}