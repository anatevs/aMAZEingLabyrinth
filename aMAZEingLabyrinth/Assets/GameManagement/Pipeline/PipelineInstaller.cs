using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using VContainerExt;

namespace GamePipeline
{
    public sealed class PipelineInstaller : IInitializable, IDisposable
    {
        private readonly GameplayPipeline _gameplayPipeline;

        private readonly IObjectResolver _objectResolver;

        public PipelineInstaller(GameplayPipeline gameplayPipeline, IObjectResolver objResolver)
        {
            _gameplayPipeline = gameplayPipeline;
            _objectResolver = objResolver;
        }

        void IInitializable.Initialize()
        {
            _gameplayPipeline.AddTask(ObjectResolverExtension.ResolveInstance<StartTask>(_objectResolver));
            //_turnPipeline.AddTask(ObjectResolverExtension.ResolveInstance<TurnTask>(_objectResolver));
            //_turnPipeline.AddTask(ObjectResolverExtension.ResolveInstance<HandleVisualPipelineTask>(_objectResolver));
        }

        void IDisposable.Dispose()
        {
            _gameplayPipeline.Clear();
        }
    }
}