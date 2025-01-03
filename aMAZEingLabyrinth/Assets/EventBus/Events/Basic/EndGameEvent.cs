using GameCore;

namespace EventBusNamespace
{
    public readonly struct EndGameEvent : IEvent
    {
        public readonly Player WinPlayer;

        public EndGameEvent(Player winPlayer)
        {
            WinPlayer = winPlayer;
        }
    }
}