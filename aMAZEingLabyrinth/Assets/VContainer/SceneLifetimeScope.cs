using EventBusNamespace;
using GameCore;
using GameUI;
using GamePipeline;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using GameManagement;

public class SceneLifetimeScope : LifetimeScope
{
    [SerializeField]
    private MenusService _menuWindowsService;

    [SerializeField]
    private PlayersList _playersList;

    [SerializeField]
    private ShiftArrowsService _shiftArrowsService;

    [SerializeField]
    private CellsLabyrinth _cellsLabyrinth;

    [SerializeField]
    private CellHighlight _cellHighlight;

    [SerializeField]
    private GameListenersManager _gameListenersManager;

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterEventBus(builder);

        RegisterServices(builder);

        RegisterPipeline(builder);

        RegisterGameManagement(builder);

        RegisterHandlers(builder);

        RegisterGameListeners(builder);
    }

    private void RegisterEventBus(IContainerBuilder builder)
    {
        builder.Register<EventBus>(Lifetime.Singleton);
    }

    private void RegisterServices(IContainerBuilder builder)
    {
        builder.RegisterComponent(_shiftArrowsService);
        builder.RegisterComponent(_cellsLabyrinth);
        builder.RegisterComponent(_cellHighlight);

        builder.RegisterEntryPoint<ShiftArrowsController>(Lifetime.Singleton)
            .AsSelf();
    }

    private void RegisterPipeline(IContainerBuilder builder)
    {
        builder.RegisterComponent(_menuWindowsService);

        builder.RegisterComponent(_playersList);

        builder.Register<MakeShiftPipeline>(Lifetime.Singleton);

        builder.Register<TurnPipeline>(Lifetime.Singleton);

        builder.Register<AudioVisualPipeline>(Lifetime.Singleton);

        builder.RegisterEntryPoint<PipelineInstaller>(Lifetime.Singleton);
    }

    private void RegisterGameManagement(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<StartGameManager>(Lifetime.Singleton);
        builder.RegisterEntryPoint<EndGameManager>(Lifetime.Singleton);
    }

    private void RegisterHandlers(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<MakeShiftHandler>(Lifetime.Singleton);
        builder.RegisterEntryPoint<ClickCellHandler>(Lifetime.Singleton);
        builder.RegisterEntryPoint<MoveThroughPathHandler>(Lifetime.Singleton);
        builder.RegisterEntryPoint<CheckWinHandler>(Lifetime.Singleton);
        builder.RegisterEntryPoint<NextPlayerHandler>(Lifetime.Singleton);

        builder.RegisterEntryPoint<MoveThroughPathVisualHandler>(Lifetime.Singleton);
    }

    private void RegisterGameListeners(IContainerBuilder builder)
    {
        builder.RegisterComponent(_gameListenersManager);

        builder.RegisterEntryPoint<GameListenersInstaller>(Lifetime.Singleton);
    }
}
