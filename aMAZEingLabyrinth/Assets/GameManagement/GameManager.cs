using GamePipeline;
using VContainer.Unity;
using EventBusNamespace;

namespace GameCore
{
    public class GameManager : IPostStartable
    {
        private readonly GameplayPipeline _gameplayPipeline;

        private readonly ShiftArrowsService _shiftArrowsService;

        private readonly CellsLabyrinth _cellsLabyrinth;

        private readonly EventBus _eventBus;

        public GameManager(GameplayPipeline turnPipeline,
            ShiftArrowsService shiftArrowsService,
            CellsLabyrinth cellsLabyrinth,
            EventBus eventBus)
        {
            _gameplayPipeline = turnPipeline;
            _shiftArrowsService = shiftArrowsService;
            _cellsLabyrinth = cellsLabyrinth;
            _eventBus = eventBus;
        }

        void IPostStartable.PostStart()
        {
            _gameplayPipeline.Run();
        }
    }
}