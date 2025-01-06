using GameCore;

namespace EventBusNamespace
{
    public sealed class NextPlayerHandler : BaseHandler<NextPlayerEvent>
    {
        private readonly PlayersList _playersList;

        public NextPlayerHandler(EventBus eventBus,
            PlayersList players) : base(eventBus)
        {
            _playersList = players;
        }

        protected override void RaiseEvent(NextPlayerEvent evnt)
        {
            _playersList.SetNextPlayer();

            EventBus.RaiseEvent(new HighlightRewardInfoVisualEvent(
                _playersList.CurrentPlayer.Type));
        }
    }
}