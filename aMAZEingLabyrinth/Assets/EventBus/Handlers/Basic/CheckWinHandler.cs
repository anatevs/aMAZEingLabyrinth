using GameUI;
using UnityEngine;

namespace EventBusNamespace
{
    public sealed class CheckWinHandler : BaseHandler<CheckWinEvent>
    {
        public CheckWinHandler(EventBus eventBus) : base(eventBus)
        {
        }

        protected override void RaiseEvent(CheckWinEvent evnt)
        {
            var player = evnt.Player;

            if (player.RemainTargetsCount == 0)
            {
                EventBus.RaiseEvent(new EndGameEvent(player));
            }
            else
            {
                EventBus.RaiseEvent(new NextPlayerEvent());
            }
        }
    }
}