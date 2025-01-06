using GameCore;
namespace GamePipeline
{
    public sealed class HighlightRewardInfoVisualTask : Task
    {
        private readonly PlayerType _playerType;
        private readonly RewardCardsService _rewardCardsService;

        public HighlightRewardInfoVisualTask(PlayerType playerType,
            RewardCardsService rewardCardsService)
        {
            _playerType = playerType;
            _rewardCardsService = rewardCardsService;
        }

        protected override void OnRun()
        {
            _rewardCardsService.SetActivePlayerHighlight(_playerType);

            Finish();
        }
    }
}