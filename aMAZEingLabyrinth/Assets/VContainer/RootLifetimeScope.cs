using GameCore;
using SaveLoadNamespace;
using UnityEngine;
using VContainer;
using VContainer.Unity;

public class RootLifetimeScope : LifetimeScope
{
    [SerializeField]
    private MovableCellsConfig _movableCellsConfig;

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterSaveLoads(builder);
    }

    private void RegisterSaveLoads(IContainerBuilder builder)
    {
        builder.Register<GameRepository>(Lifetime.Singleton);

        builder.Register<MovableCellsManager>(Lifetime.Singleton)
            .WithParameter(_movableCellsConfig);

        builder.Register<MovingCellsSaveLoad>(Lifetime.Singleton)
            .AsImplementedInterfaces();

        builder.RegisterEntryPoint<SaveLoadManager>(Lifetime.Singleton);
    }
}