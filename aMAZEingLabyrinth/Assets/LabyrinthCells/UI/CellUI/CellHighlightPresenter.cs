using UnityEngine;

namespace GameCore
{
    public sealed class CellHighlightPresenter : MonoBehaviour
    {
        [SerializeField]
        private CellsCollection _cellsCollection;

        [SerializeField]
        CellHighlightView _view;

        private Vector3 _currentPos = new();

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

        private void SetHighlight(Vector3 mousePos)
        {
            var pos = _cellsCollection.GetCellCenterCoordinates(mousePos);

            if (_currentPos == pos)
            {
                return;
            }

            _currentPos = pos;

            _view.SetHighlight(pos);
        }

        private void ClickOnCell()
        {
            _cellsCollection.FindPath(_currentPos, out _);
        }
    }
}