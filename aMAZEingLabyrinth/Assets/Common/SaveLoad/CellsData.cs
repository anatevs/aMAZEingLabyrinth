using GameCore;
using System.Collections.Generic;

namespace SaveLoadNamespace
{
    public class CellsData
    {
        public List<OneCellData> MovableCellsData
        {
            get => _movableCellsData;
            set => _movableCellsData = value;
        }

        public OneCellData PlayableCellData
        {
            get => _playableCellData;
            set => _playableCellData = value;
        }

        private List<OneCellData> _movableCellsData = new();

        private OneCellData _playableCellData;

        public void AddMovableCell(OneCellData oneCellData)
        {
            _movableCellsData.Add(oneCellData);
        }

        public void ClearMovableData()
        {
            _movableCellsData.Clear();
        }

        public void SetPlayableCell(OneCellData playableCellData)
        {
            _playableCellData = playableCellData;
        }
    }
}