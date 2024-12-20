using EventBusNamespace;
using GamePipeline;
using System;
using VContainer.Unity;

namespace GameCore
{
    public sealed class ShiftArrowsController : IInitializable, IDisposable
    {
        public (int row, int col) ClickedIndex => _clickedIndex;

        private readonly ShiftArrowsService _arrowsService;

        private readonly EventBus _eventBus;

        private readonly MakeShiftPipeline _makeShiftPipeline;

        private (int row, int col) _clickedIndex;

        public ShiftArrowsController(ShiftArrowsService arrowsService,
            EventBus eventBus,
            MakeShiftPipeline makeShiftPipeline)
        {
            _arrowsService = arrowsService;
            _eventBus = eventBus;
            _makeShiftPipeline = makeShiftPipeline;
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
            _clickedIndex = (row, col);

            _makeShiftPipeline.Run();
            //_eventBus.RaiseEvent(new MakeShiftEvent(row, col));
        }
    }
}