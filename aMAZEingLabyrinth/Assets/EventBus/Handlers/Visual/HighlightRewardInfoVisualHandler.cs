using GameCore;
using GamePipeline;

namespace EventBusNamespace
{
    public sealed class HighlightRewardInfoVisualHandler :
        VisualHandler<HighlightRewardInfoVisualEvent>
    {
        private readonly RewardCardsService _rewardCardsService;

        public HighlightRewardInfoVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline,
            RewardCardsService rewardCardsService)
            : base(eventBus, visualPipeline)
        {
            _rewardCardsService = rewardCardsService;
        }

        protected override void RaiseEvent(HighlightRewardInfoVisualEvent evnt)
        {
            VisualPipeline.AddTask(new HighlightRewardInfoVisualTask(
                evnt.PlayerType, _rewardCardsService));
        }
    }
}