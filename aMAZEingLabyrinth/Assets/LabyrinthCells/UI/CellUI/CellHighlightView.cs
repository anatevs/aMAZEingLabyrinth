using System;
using UnityEngine;
using GameModules;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace GameCore
{
    [RequireComponent(typeof(BoxCollider2D))]
    public sealed class CellHighlightView : MonoBehaviour
    {
        public event Action<Vector3> OnMouseEnter;

        public event Action OnMouseExit;

        public event Action OnCellClicked;

        [SerializeField]
        private GameObject _highlightImage;

        [SerializeField]
        private float _clickVisualDuration = 0.1f;

        [SerializeField]
        private float _clickScale = 0.8f;

        private bool _isShowingClick = false;

        private bool _isActive = false;

        private BoxCollider2D _boxCollider;

        private InputSystem _inputSystem;

        private bool _isOverlapping = false;

        private void Awake()
        {
            _boxCollider = GetComponent<BoxCollider2D>();

            _inputSystem = new();
        }

        private void OnEnable()
        {
            OnCellClicked += PlayClickVisual;
        }

        private void OnDisable()
        {
            OnCellClicked -= PlayClickVisual;
        }

        private void Update()
        {
            var mousePos = _inputSystem.GetMousePos();

            if (_boxCollider.OverlapPoint(mousePos))
            {
                _isOverlapping = true;

                if (!_isShowingClick)
                {
                    OnMouseEnter?.Invoke(mousePos);

                    if (_inputSystem.IsMouseClicked())
                    {
                        OnCellClicked?.Invoke();
                    }
                }
            }
            else
            {
                if (_isOverlapping && _highlightImage.activeSelf)
                {
                    OnMouseExit?.Invoke();
                }

                _isOverlapping = false;

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
            _isActive = isActive;

            if (!_isShowingClick)
            {
                _highlightImage.SetActive(isActive);
            }
        }

        public async void PlayClickVisual()
        {
            _isShowingClick = true;
            Debug.Log($"start click: {Time.time}");
            await DOTween.Sequence()
                .Append(_highlightImage.transform.DOScale(_clickScale, _clickVisualDuration))
                .Append(_highlightImage.transform.DOScale(1, _clickVisualDuration))
                .AsyncWaitForCompletion();
            Debug.Log($"end click: {Time.time}");
            _isShowingClick = false;

            _highlightImage.SetActive(_isActive);
        }
    }
}