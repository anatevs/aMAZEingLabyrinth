using EventBusNamespace;
using GameCore;
using GamePipeline;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class SceneLifetimeScope : LifetimeScope
{
    [SerializeField]
    private PlayerSelector _playerSelector;

    [SerializeField]
    private PlayersList _playersList;

    [SerializeField]
    private ShiftArrowsService _shiftArrowsService;

    [SerializeField]
    private CellsLabyrinth _cellsLabyrinth;

    [SerializeField]
    private CellHighlight _cellHighlight;

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterEventBus(builder);

        RegisterServices(builder);

        RegisterPipeline(builder);

        RegisterGameManagement(builder);

        RegisterHandlers(builder);
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
    }

    private void RegisterPipeline(IContainerBuilder builder)
    {
        builder.RegisterComponent(_playerSelector);
        builder.RegisterComponent(_playersList);

        builder.Register<TurnPipeline>(Lifetime.Singleton);

        builder.RegisterEntryPoint<PipelineInstaller>(Lifetime.Singleton);
    }

    private void RegisterGameManagement(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<GameManager>(Lifetime.Singleton);
    }

    private void RegisterHandlers(IContainerBuilder builder)
    {
        builder.RegisterEntryPoint<MakeShiftHandler>(Lifetime.Singleton);
    }
}
