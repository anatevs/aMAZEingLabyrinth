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
        }

        private void OnDisable()
        {
            _view.OnMouseEnter -= SetHighlight;
        }

        private void SetHighlight(Vector3 mousePos)
        {
            var pos = _cellsCollection.GetCellCoordinates(mousePos);

            if (_currentPos == pos)
            {
                return;
            }

            _currentPos = pos;

            _view.SetHighlight(pos);
        }
    }
}