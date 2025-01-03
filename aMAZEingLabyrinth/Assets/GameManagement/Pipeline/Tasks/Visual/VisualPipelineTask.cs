namespace GamePipeline
{
    public sealed class VisualPipelineTask : Task
    {
        private readonly AudioVisualPipeline _visualPipeline;

        public VisualPipelineTask(AudioVisualPipeline visualPipeline)
        {
            _visualPipeline = visualPipeline;
        }

        protected override void OnRun()
        {
            _visualPipeline.OnFinished += OnFinishVisualPipelineTask;

            _visualPipeline.Run();
        }

        protected override void OnFinished()
        {
            _visualPipeline.OnFinished -= OnFinishVisualPipelineTask;
        }

        private void OnFinishVisualPipelineTask()
        {
            _visualPipeline.Clear();

            Finish();
        }
    }
}