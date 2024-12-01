using System;
using VContainer;
using VContainer.Unity;
using VContainerExt;

namespace GamePipeline
{
    public sealed class PipelineInstaller : IInitializable, IDisposable
    {
        private readonly TurnPipeline _turnPipeline;

        private readonly IObjectResolver _objectResolver;

        public PipelineInstaller(TurnPipeline gameplayPipeline, IObjectResolver objResolver)
        {
            _turnPipeline = gameplayPipeline;
            _objectResolver = objResolver;
        }

        void IInitializable.Initialize()
        {
            _turnPipeline.AddTask(ObjectResolverExtension.ResolveInstance<TurnTask>(_objectResolver));
            //_turnPipeline.AddTask(ObjectResolverExtension.ResolveInstance<HandleVisualPipelineTask>(_objectResolver));
        }

        void IDisposable.Dispose()
        {
            _turnPipeline.Clear();
        }
    }
}