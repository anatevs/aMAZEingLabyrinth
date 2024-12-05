using GameCore;
using SaveLoadNamespace;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
    [SerializeField]
    private MovableCellsConfig _movableCellsConfig;

    [SerializeField]
    private PlayersDataConfig _playersConfig;

    [SerializeField]
    private CellPrefabsConfig _cellPrefabsConfig;

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterCellsPool(builder);

        RegisterSaveLoadSystem(builder);
    }

    private void RegisterCellsPool(IContainerBuilder builder)
    {
        var cellsPool = new CellsPool(_cellPrefabsConfig, _movableCellsConfig);

        builder.RegisterComponent(cellsPool);
    }

    private void RegisterSaveLoadSystem(IContainerBuilder builder)
    {
        builder.Register<GameRepository>(Lifetime.Singleton);

        RegisterDataConnectors(builder);

        RegisterSaveLoads(builder);

        builder.RegisterEntryPoint<SaveLoadManager>(Lifetime.Singleton)
            .AsSelf();
    }

    private void RegisterDataConnectors(IContainerBuilder builder)
    {
        builder.Register<UnfixedCellsDataConnector>(Lifetime.Singleton)
            .WithParameter(_movableCellsConfig);

        builder.Register<PlayersDataConnector>(Lifetime.Singleton)
            .WithParameter(_playersConfig);

        builder.Register<ShiftArrowsDataConnector>(Lifetime.Singleton);
    }

    private void RegisterSaveLoads(IContainerBuilder builder)
    {
        builder.Register<UnfixedCellsSaveLoad>(Lifetime.Singleton)
            .AsImplementedInterfaces();

        builder.Register<PlayersSaveLoad>(Lifetime.Singleton)
            .AsImplementedInterfaces();

        builder.Register<ShiftArrowsSaveLoad>(Lifetime.Singleton)
            .AsImplementedInterfaces();
    }
}