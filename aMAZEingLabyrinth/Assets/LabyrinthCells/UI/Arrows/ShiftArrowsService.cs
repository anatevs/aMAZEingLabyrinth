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

        private ShiftArrow _disabledArrow;

        private readonly Dictionary<(int, int), ShiftArrow> _indexArrows = new();

        private void Start()
        {
            foreach (ShiftArrow arrow in _arrows)
            {
                _indexArrows.Add(arrow.Index, arrow);

                arrow.OnShift += Click;
            }
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

            if (_disabledArrow != null)
            {
                _disabledArrow.ActivateButton();
            }

            newDisabled.DisactivateButton();

            _disabledArrow = newDisabled;
        }
    }
}