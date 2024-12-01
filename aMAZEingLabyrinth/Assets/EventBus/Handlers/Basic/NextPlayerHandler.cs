using GameCore;

namespace EventBusNamespace
{
    public sealed class NextPlayerHandler : BaseHandler<NextPlayerEvent>
    {
        private readonly PlayersList _playersList;
        private readonly ShiftArrowsService _shiftArrows;
        private readonly CellHighlight _cellHighlight;

        public NextPlayerHandler(EventBus eventBus,
            PlayersList players,
            ShiftArrowsService shiftArrows,
            CellHighlight cellHighlight) : base(eventBus)
        {
            _playersList = players;
            _shiftArrows = shiftArrows;
            _cellHighlight = cellHighlight;
        }

        protected override void RaiseEvent(NextPlayerEvent evnt)
        {
            _playersList.SetNextPlayer();

            _shiftArrows.EnableAllActiveArrows();

            _cellHighlight.SetActive(true);
        }
    }
}