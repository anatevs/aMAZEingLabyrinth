using GameCore;

namespace GamePipeline
{
    public sealed class ReleaseRewardVisualTask : Task
    {
        private readonly Player _player;
        private readonly RewardCardsService _rewardCardsService;

        public ReleaseRewardVisualTask(Player player,
            RewardCardsService rewardCardsService)
        {
            _player = player;
            _rewardCardsService = rewardCardsService;
        }

        protected override async void OnRun()
        {
            await _rewardCardsService.SetNewTarget(_player);

            Finish();
        }
    }
}