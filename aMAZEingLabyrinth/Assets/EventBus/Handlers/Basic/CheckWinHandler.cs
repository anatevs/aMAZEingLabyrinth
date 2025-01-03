using GameUI;
using UnityEngine;

namespace EventBusNamespace
{
    public sealed class CheckWinHandler : BaseHandler<CheckWinEvent>
    {
        private readonly MenusService _menus;

        public CheckWinHandler(EventBus eventBus, MenusService menus) : base(eventBus)
        {
            _menus = menus;
        }

        protected override void RaiseEvent(CheckWinEvent evnt)
        {
            var player = evnt.Player;

            player.ReleaseReward();

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