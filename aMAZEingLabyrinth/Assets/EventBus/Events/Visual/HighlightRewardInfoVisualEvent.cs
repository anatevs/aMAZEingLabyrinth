using GameCore;

namespace EventBusNamespace
{
    public readonly struct HighlightRewardInfoVisualEvent : IEvent
    {
        public readonly PlayerType PlayerType;

        public HighlightRewardInfoVisualEvent(PlayerType playerType)
        {
            PlayerType = playerType;
        }
    }
}