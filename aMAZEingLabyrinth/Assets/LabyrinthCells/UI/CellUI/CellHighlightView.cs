using System;
using UnityEngine;

namespace GameCore
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class CellHighlightView : MonoBehaviour
    {
        public event Action<Vector3> OnMouseEnter;

        public event Action OnCellClicked;

        [SerializeField]
        private GameObject _highlightImage;

        private BoxCollider2D _boxCollider;

        private InputSystem _inputSystem;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();

            _inputSystem = new();
        }

        private void Update()
        {
            var mousePos = _inputSystem.GetMousePos();

            if (_boxCollider.OverlapPoint(mousePos))
            {
                OnMouseEnter?.Invoke(mousePos);

                if (_inputSystem.IsMouseClicked())
                {
                    OnCellClicked?.Invoke();
                }
            }
            else
            {
                _highlightImage.SetActive(false);
            }
        }

        public void SetHighlight(Vector3 pos)
        {
            _highlightImage.SetActive(true);
            _highlightImage.transform.position = pos;
        }

        public void SetActive(bool isActive)
        {
            _highlightImage.SetActive(isActive);
        }
    }
}