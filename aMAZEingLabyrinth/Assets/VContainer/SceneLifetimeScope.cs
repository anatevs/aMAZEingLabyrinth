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

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterPipeline(builder);

        RegisterGameManagement(builder);
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
}
