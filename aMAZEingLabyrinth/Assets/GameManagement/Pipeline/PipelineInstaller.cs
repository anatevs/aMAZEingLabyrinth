using System;
using VContainer;
using VContainer.Unity;
using VContainerExt;

namespace GamePipeline
{
    public sealed class PipelineInstaller : IInitializable, IDisposable
    {
        private readonly MakeShiftPipeline _makeShiftPipeline;
        private readonly TurnPipeline _turnPipeline;

        private readonly IObjectResolver _objectResolver;

        public PipelineInstaller(MakeShiftPipeline makeShiftPipeline,
            TurnPipeline gameplayPipeline, IObjectResolver objResolver)
        {
            _makeShiftPipeline = makeShiftPipeline;
            _turnPipeline = gameplayPipeline;
            _objectResolver = objResolver;
        }

        void IInitializable.Initialize()
        {
            _makeShiftPipeline.AddTask(ObjectResolverExtension.ResolveInstance<MakeShiftTask>(_objectResolver));

            _turnPipeline.AddTask(ObjectResolverExtension.ResolveInstance<TurnTask>(_objectResolver));
            _turnPipeline.AddTask(ObjectResolverExtension.ResolveInstance<VisualPipelineTask>(_objectResolver));
        }

        void IDisposable.Dispose()
        {
            _turnPipeline.Clear();
        }
    }
}