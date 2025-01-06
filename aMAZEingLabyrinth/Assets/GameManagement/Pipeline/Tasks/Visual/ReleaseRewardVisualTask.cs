using GameCore;

namespace GamePipeline
{
    public class ReleaseRewardVisualTask : Task
    {
        private readonly Player _player;
        private readonly RewardCardsService _rewardCardsService;

        public ReleaseRewardVisualTask(Player player,
            RewardCardsService rewardCardsService)
        {
            _player = player;
            _rewardCardsService = rewardCardsService;
        }

        protected override void OnRun()
        {
            _rewardCardsService.SetTargetUIInfo(_player);

            Finish();
        }
    }
}