using System;
using UnityEngine;

namespace GameCore
{
    public class ShiftArrowsService : MonoBehaviour
    {
        public event Action<int, int> OnClick;

        public event Action<ShiftArrow> OnButtonDisactivated;

        [SerializeField]
        private ShiftArrow[] _arrows;

        private ShiftArrow _disabledArrow;

        private void Start()
        {
            foreach (ShiftArrow arrow in _arrows)
            {
                arrow.OnShift += Click;

                arrow.OnDisactiveted += ChangeDisabledArrow;
            }
        }

        private void Click(int row, int column)
        {
            OnClick?.Invoke(row, column);
        }

        private void ChangeDisabledArrow(ShiftArrow newDisabled)
        {
            if (_disabledArrow != null)
            {
                _disabledArrow.EnableButton();
            }

            _disabledArrow = newDisabled;
        }

        private void OnDisable()
        {
            foreach (ShiftArrow arrow in _arrows)
            {
                arrow.OnShift -= OnClick;

                arrow.OnDisactiveted -= ChangeDisabledArrow;
            }
        }
    }
}