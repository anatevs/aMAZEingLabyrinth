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

        protected override async void OnRun()
        {
            await _noPathWindow.SetActiveTask();

            Finish();
        }
    }
}