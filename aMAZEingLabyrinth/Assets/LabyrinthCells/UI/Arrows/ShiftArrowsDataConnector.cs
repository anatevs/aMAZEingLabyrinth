using GameCore;
using System;

namespace SaveLoadNamespace
{
    public sealed class ShiftArrowsDataConnector
    {
        public event Action OnArrowsInfoRequested;

        public ShiftArrowsData Data => _data;

        public (int, int) DisabledIndex => _disabledIndex;

        public bool AreActive => _areActive;

        private ShiftArrowsData _data;

        private (int, int) _disabledIndex;
        private bool _areActive;

        public void SetArrowsData(GameCore.ShiftArrowsData data)
        {
            _data = data;
        }

        public void SetArrows((int, int) disabledIndex, bool areActive)
        {
            _areActive = areActive;
            _disabledIndex = disabledIndex;
        }

        public void SetupArrows()
        {
            OnArrowsInfoRequested?.Invoke();
        }
    }
}