using GameUI;

namespace EventBusNamespace
{
    public readonly struct ShowNoPathVisualEvent : IEvent
    {
        public readonly NoPathMessageWindow NoPathWindow;

        public ShowNoPathVisualEvent(NoPathMessageWindow noPathWindow)
        {
            NoPathWindow = noPathWindow;
        }
    }
}