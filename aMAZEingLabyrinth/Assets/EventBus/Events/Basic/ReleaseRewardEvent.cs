using GameCore;

namespace EventBusNamespace
{
    public readonly struct ReleaseRewardEvent : IEvent
    {
        public readonly Player Player;

        public ReleaseRewardEvent(Player player)
        {
            Player = player;
        }
    }
}