using GamePipeline;
using System;
using VContainer.Unity;
using EventBusNamespace;

namespace GameCore
{
    public class GameManager : IInitializable, IPostStartable, IDisposable
    {
        private readonly TurnPipeline _turnPipeline;

        private readonly ShiftArrowsService _shiftArrowsService;

        private readonly CellsLabyrinth _cellsLabyrinth;

        private readonly EventBus _eventBus;

        public GameManager(TurnPipeline turnPipeline,
            ShiftArrowsService shiftArrowsService,
            CellsLabyrinth cellsLabyrinth,
            EventBus eventBus)
        {
            _turnPipeline = turnPipeline;
            _shiftArrowsService = shiftArrowsService;
            _cellsLabyrinth = cellsLabyrinth;
            _eventBus = eventBus;
        }

        void IInitializable.Initialize()
        {
            _shiftArrowsService.OnClick += ArrowClick;
        }

        void IDisposable.Dispose()
        {
            _shiftArrowsService.OnClick -= ArrowClick;
        }

        void IPostStartable.PostStart()
        {
            _turnPipeline.Run();
        }

        private void ArrowClick(int row, int col)
        {
            _eventBus.RaiseEvent(new MakeShiftEvent(
                _shiftArrowsService, _cellsLabyrinth, row, col));
        }
    }
}