using GameCore;

namespace EventBusNamespace
{
    public readonly struct CheckWinEvent : IEvent
    {
        public readonly Player Player;
        public CheckWinEvent(Player initPlayer)
        {
            Player = initPlayer;
        }
    }
}