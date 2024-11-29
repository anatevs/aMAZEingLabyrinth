using EventBusNamespace;
using UnityEngine;
using VContainer;

namespace GameCore
{
    public sealed class CellHighlight : MonoBehaviour
    {
        public Vector3 CurrentPosition => _currentPos;

        [SerializeField]
        private CellsLabyrinth _labyrinth;

        [SerializeField]
        CellHighlightView _view;

        private bool _isActive;

        private Vector3 _currentPos = new();

        private EventBus _eventBus;

        [Inject]
        public void Construct(EventBus eventBus)
        {
            _eventBus = eventBus;
        }

        private void Awake()
        {
            _view.OnMouseEnter += SetHighlight;

            _view.OnCellClicked += ClickOnCell;
        }

        private void OnDisable()
        {
            _view.OnMouseEnter -= SetHighlight;

            _view.OnCellClicked -= ClickOnCell;
        }

        public void SetActive(bool isActive)
        {
            _isActive = isActive;
        }

        private void SetHighlight(Vector3 mousePos)
        {
            if (_isActive)
            {
                var pos = _labyrinth.GetCellCenterCoordinates(mousePos);

                if (_currentPos == pos)
                {
                    return;
                }

                _currentPos = pos;

                _view.SetHighlight(pos);
            }
        }

        private void ClickOnCell()
        {
            if (_isActive)
            {
                _eventBus.RaiseEvent(new ClickCellEvent(this));
                //_labyrinth.FindPath(_currentPos, out _);
            }
        }
    }
}