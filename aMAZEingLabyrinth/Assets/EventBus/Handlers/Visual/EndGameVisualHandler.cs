using GamePipeline;
using GameUI;

namespace EventBusNamespace
{
    public sealed class EndGameVisualHandler : VisualHandler<EndGameEvent>
    {
        private readonly EndGameMenu _endGameMenu;

        public EndGameVisualHandler(EventBus eventBus,
            AudioVisualPipeline visualPipeline,
            MenusService menusService) 
            : base(eventBus, visualPipeline)
        {
            _endGameMenu = menusService.EndGame;
        }

        protected override void RaiseEvent(EndGameEvent evnt)
        {
            VisualPipeline.AddTask(new EndGameVisualTask(
                evnt.WinPlayer, _endGameMenu));
        }
    }
}