using GameCore;
using System.Collections.Generic;

namespace EventBusNamespace
{
    public readonly struct MoveThroughPathEvent : IEvent
    {
        public readonly Player Player;
        public readonly List<(int x, int y)> Path;

        public MoveThroughPathEvent(Player player, List<(int x, int y)> path)
        {
            Player = player;
            Path = path;
        }
    }
}