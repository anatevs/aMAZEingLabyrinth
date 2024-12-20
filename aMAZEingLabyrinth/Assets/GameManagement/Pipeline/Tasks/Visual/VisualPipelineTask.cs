using GameCore;

namespace GamePipeline
{
    public sealed class VisualPipelineTask : Task
    {
        private readonly AudioVisualPipeline _visualPipeline;
        private readonly CellHighlight _cellHighlight;

        public VisualPipelineTask(AudioVisualPipeline visualPipeline, CellHighlight cellHighlight)
        {
            _visualPipeline = visualPipeline;
            _cellHighlight = cellHighlight;
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

            _cellHighlight.SetActive(true);
        }
    }
}