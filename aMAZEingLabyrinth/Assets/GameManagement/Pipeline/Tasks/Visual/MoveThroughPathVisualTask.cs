using GameCore;
using System.Collections.Generic;

namespace GamePipeline
{
    public sealed class MoveThroughPathVisualTask : Task
    {
        private readonly Player _player;
        private readonly List<(int x, int y)> _path;

        public MoveThroughPathVisualTask(Player player,
            List<(int x, int y)> path)
        {
            _player = player;
            _path = path;
        }

        protected override async void OnRun()
        {
            await _player.MoveThroughPathVisual(_path);

            Finish();
        }
    }
}