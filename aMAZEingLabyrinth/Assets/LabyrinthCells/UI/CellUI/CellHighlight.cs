using GamePipeline;
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

        private TurnPipeline _turnPipeline;

        [Inject]
        public void Construct(TurnPipeline turnPipeline)
        {
            _turnPipeline = turnPipeline;
        }

        private void OnEnable()
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
            _view.SetActive(isActive);
        }

        private void SetHighlight(Vector3 mousePos)
        {
            if (_isActive)
            {
                var pos = _labyrinth.GetCellCenterCoordinates(mousePos);

                _currentPos = pos;

                _view.SetHighlight(pos);
            }
        }

        private void ClickOnCell()
        {
            if (_isActive)
            {
                _turnPipeline.Run();
            }
        }
    }
}