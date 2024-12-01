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

            player.ReleaseReward();

            if (player.RemainTargetsCount == 0)
            {
                Debug.Log($"this game is end, the winner is {player.Type} player");
                //end game
            }
            else
            {
                Debug.Log("next player");

                EventBus.RaiseEvent(new NextPlayerEvent());
            }
        }
    }
}