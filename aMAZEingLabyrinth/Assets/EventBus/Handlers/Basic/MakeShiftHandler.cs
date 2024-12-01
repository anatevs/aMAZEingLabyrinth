using GameCore;

namespace EventBusNamespace
{
    public sealed class MakeShiftHandler : BaseHandler<MakeShiftEvent>
    {
        private readonly ShiftArrowsService _shiftArrowsService;
        private readonly CellsLabyrinth _cellsLabyrinth;

        public MakeShiftHandler(EventBus eventBus, ShiftArrowsService shiftArrowsService, CellsLabyrinth cellsLabyrinth) : base(eventBus)
        {
            _shiftArrowsService = shiftArrowsService;
            _cellsLabyrinth = cellsLabyrinth;
        }

        protected override void RaiseEvent(MakeShiftEvent evnt)
        {
            _cellsLabyrinth.MakeShift(evnt.Row, evnt.Col, out var oppositeIndex);

            _shiftArrowsService.ChangeDisabledArrow(oppositeIndex);

            _shiftArrowsService.DisableAllArrows();
        }
    }
}