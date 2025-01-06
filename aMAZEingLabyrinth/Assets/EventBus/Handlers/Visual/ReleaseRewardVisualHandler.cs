using GameCore;
using GamePipeline;

namespace EventBusNamespace
{
    public sealed class ReleaseRewardVisualHandler : BaseHandler<ReleaseRewardEvent>
    {
        private readonly AudioVisualPipeline _visualPipeline;
        private readonly RewardCardsService _rewardCardsService;
        private readonly ShiftArrowsService _shiftArrows;
        private readonly CellHighlight _cellHighlight;

        public ReleaseRewardVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline,
            RewardCardsService rewardCardsService,
            ShiftArrowsService shiftArrows,
            CellHighlight cellHighlight) : base(eventBus)
        {
            _visualPipeline = visualPipeline;
            _rewardCardsService = rewardCardsService;
            _shiftArrows = shiftArrows;
            _cellHighlight = cellHighlight;
        }

        protected override void RaiseEvent(ReleaseRewardEvent evnt)
        {
            _visualPipeline.AddTask(new SetActiveBoardUITask(
                _shiftArrows, _cellHighlight, false));

            _visualPipeline.AddTask(new ReleaseRewardVisualTask(
                evnt.Player, _rewardCardsService));

            _visualPipeline.AddTask(new SetActiveBoardUITask(
                _shiftArrows, _cellHighlight, true));
        }
    }
}