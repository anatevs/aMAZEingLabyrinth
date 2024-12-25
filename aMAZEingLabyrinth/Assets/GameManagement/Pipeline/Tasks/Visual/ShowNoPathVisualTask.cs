using GameUI;

namespace GamePipeline
{
    public sealed class ShowNoPathVisualTask : Task
    {
        private readonly NoPathMessageWindow _noPathWindow;

        public ShowNoPathVisualTask(NoPathMessageWindow noPathWindow)
        {
            _noPathWindow = noPathWindow;
        }

        protected override void OnRun()
        {
            _noPathWindow.SetActive();

            Finish();
        }
    }
}