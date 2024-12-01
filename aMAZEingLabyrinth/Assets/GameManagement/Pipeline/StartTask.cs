namespace GamePipeline
{
    public sealed class StartTask : Task
    {
        protected override void OnRun()
        {
            Finish();
        }

        protected override void OnFinished()
        {
        }
    }
}