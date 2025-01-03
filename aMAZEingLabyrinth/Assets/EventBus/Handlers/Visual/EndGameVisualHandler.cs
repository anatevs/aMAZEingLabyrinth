using GamePipeline;
using GameUI;

namespace EventBusNamespace
{
    public sealed class EndGameVisualHandler : BaseHandler<EndGameEvent>
    {
        private readonly AudioVisualPipeline _visualPipeline;
        private readonly EndGameMenu _endGameMenu;

        public EndGameVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline,
            MenusService menusService) : base(eventBus)
        {
            _visualPipeline = visualPipeline;
            _endGameMenu = menusService.EndGame;
        }

        protected override void RaiseEvent(EndGameEvent evnt)
        {
            _visualPipeline.AddTask(new EndGameVisualTask(
                evnt.WinPlayer, _endGameMenu));
        }
    }
}