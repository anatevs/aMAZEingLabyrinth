using Cysharp.Threading.Tasks;
using GameCore;
using System.Collections.Generic;

namespace GamePipeline
{
    public sealed class MoveThroughPathVisualTask : Task
    {
        private readonly Player _player;
        private readonly List<(int x, int y)> _path;
        private readonly CellHighlight _cellHighlight;

        public MoveThroughPathVisualTask(Player player,
            List<(int x, int y)> path,
            CellHighlight cellHighlight)
        {
            _player = player;
            _path = path;
            _cellHighlight = cellHighlight;
        }

        protected override async void OnRun()
        {
            await _player.MoveThroughPathVisual(_path);

            _cellHighlight.SetActive(true);

            Finish();
        }
    }
}