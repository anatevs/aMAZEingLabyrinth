using GameCore;

namespace GamePipeline
{
    public sealed class HidePathMarkersVisualTask : Task
    {
        private readonly PathMarkersPool _pool;

        public HidePathMarkersVisualTask(PathMarkersPool pool)
        {
            _pool = pool;
        }

        protected override void OnRun()
        {
            _pool.UnspawnAll();

            Finish();
        }
    }
}