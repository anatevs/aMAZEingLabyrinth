using GameCore;

namespace EventBusNamespace
{
    public sealed class ReleaseRewardHandler : BaseHandler<ReleaseRewardEvent>
    {
        private readonly RewardCardsService _rewardCardsService;

        public ReleaseRewardHandler(EventBus eventBus,
            RewardCardsService rewardCardsService) : base(eventBus)
        {
            _rewardCardsService = rewardCardsService;
        }

        protected override void RaiseEvent(ReleaseRewardEvent evnt)
        {
            var player = evnt.Player;

            player.ReleaseReward();

            EventBus.RaiseEvent(new CheckWinEvent(player));
        }
    }
}