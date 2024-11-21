using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    [RequireComponent(typeof(Button))]
    public class ShiftArrow : MonoBehaviour
    {
        public event Action<int, int> OnShift;
        public event Action<ShiftArrow> OnDisactiveted;

        [SerializeField]
        private int _row;

        [SerializeField]
        private int _column;

        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(ActClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ActClick);
        }

        public void DisableButton()
        {
            _button.enabled = false;
            OnDisactiveted?.Invoke(this);
        }

        public void EnableButton()
        {
            _button.enabled = true;
        }

        private void ActClick()
        {
            OnShift?.Invoke(_row, _column);
        }
    }
}