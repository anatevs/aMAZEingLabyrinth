using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameCore
{
    [RequireComponent(typeof(Button))]
    public class ShiftArrow : MonoBehaviour
    {
        public event Action<int, int> OnShift;

        public (int Row, int Col) Index => (_row, _column);

        [SerializeField]
        private int _row;

        [SerializeField]
        private int _column;

        private Button _button;

        private void OnEnable()
        {
            _button = GetComponent<Button>();

            _button.onClick.AddListener(ActClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(ActClick);
        }

        public void DisactivateButton()
        {
            _button.interactable = false;
        }

        public void ActivateButton()
        {
            _button.interactable = true;
        }

        private void ActClick()
        {
            OnShift?.Invoke(_row, _column);
        }
    }
}