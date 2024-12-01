using GameUI;
using UnityEngine;

namespace EventBusNamespace
{
    public sealed class CheckWinHandler : BaseHandler<CheckWinEvent>
    {
        private MenusService _menus;

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
                _menus.EndGame.SetWinner(player.Type.ToString());
                _menus.EndGame.Show();
            }
            else
            {
                Debug.Log("next player");

                EventBus.RaiseEvent(new NextPlayerEvent());
            }
        }
    }
}