using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class ShiftArrowsService : MonoBehaviour
    {
        public event Action<int, int> OnClick;

        [SerializeField]
        private ShiftArrow[] _arrows;

        private (int, int) _initIndex = (-1, -1);

        private (int, int) _disabledIndex;

        private readonly Dictionary<(int, int), ShiftArrow> _indexArrows = new();

        private void Start()
        {
            foreach (ShiftArrow arrow in _arrows)
            {
                _indexArrows.Add(arrow.Index, arrow);

                arrow.OnShift += Click;
            }

            StartGameInitArrows();
        }

        private void StartGameInitArrows()
        {
            _disabledIndex = _initIndex;
            DisableAllArrows();
        }

        private void OnDisable()
        {
            foreach (ShiftArrow arrow in _arrows)
            {
                arrow.OnShift -= Click;
            }
        }

        private void Click(int row, int column)
        {
            OnClick?.Invoke(row, column);
        }

        public void ChangeDisabledArrow((int, int) index)
        {
            var newDisabled = _indexArrows[index];

            if (_disabledIndex != _initIndex)
            {
                var prevDisabled = _indexArrows[_disabledIndex];
                prevDisabled.ActivateButton();
            }

            newDisabled.DisactivateButton();

            _disabledIndex = index;
        }

        public void DisableAllArrows()
        {
            foreach(var arrow in _arrows)
            {
                arrow.DisactivateButton();
            }
        }

        public void EnableAllActiveArrows()
        {
            foreach (var index in _indexArrows.Keys)
            {
                if (index != _disabledIndex)
                {
                    _indexArrows[index].ActivateButton();
                }
            }
        }
    }
}