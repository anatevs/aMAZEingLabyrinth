using EventBusNamespace;
using System;
using VContainer.Unity;

namespace GameCore
{
    public sealed class ShiftArrowsController : IInitializable, IDisposable
    {
        private readonly ShiftArrowsService _arrowsService;

        private readonly CellsLabyrinth _cellsLabyrinth;

        private readonly EventBus _eventBus;

        public ShiftArrowsController(ShiftArrowsService arrowsService,
            CellsLabyrinth cellsLabyrinth, EventBus eventBus)
        {
            _arrowsService = arrowsService;
            _cellsLabyrinth = cellsLabyrinth;
            _eventBus = eventBus;
        }

        void IInitializable.Initialize()
        {
            _arrowsService.OnClick += ArrowClick;
        }

        void IDisposable.Dispose()
        {
            _arrowsService.OnClick -= ArrowClick;
        }

        private void ArrowClick(int row, int col)
        {
            _eventBus.RaiseEvent(new MakeShiftEvent(row, col));
        }
    }
}