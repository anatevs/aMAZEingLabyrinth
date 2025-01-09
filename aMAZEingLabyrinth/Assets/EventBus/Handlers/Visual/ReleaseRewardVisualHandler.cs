using GameCore;
using GamePipeline;

namespace EventBusNamespace
{
    public sealed class ReleaseRewardVisualHandler : VisualHandler<ReleaseRewardEvent>
    {
        private readonly RewardCardsService _rewardCardsService;
        private readonly ShiftArrowsService _shiftArrows;
        private readonly CellHighlight _cellHighlight;

        public ReleaseRewardVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline,
            RewardCardsService rewardCardsService,
            ShiftArrowsService shiftArrows,
            CellHighlight cellHighlight)
            : base(eventBus, visualPipeline)
        {
            _rewardCardsService = rewardCardsService;
            _shiftArrows = shiftArrows;
            _cellHighlight = cellHighlight;
        }

        protected override void RaiseEvent(ReleaseRewardEvent evnt)
        {
            VisualPipeline.AddTask(new SetActiveBoardUITask(
                _shiftArrows, _cellHighlight, false));

            VisualPipeline.AddTask(new ReleaseRewardVisualTask(
                evnt.Player, _rewardCardsService));

            VisualPipeline.AddTask(new SetActiveBoardUITask(
                _shiftArrows, _cellHighlight, true));
        }
    }
}