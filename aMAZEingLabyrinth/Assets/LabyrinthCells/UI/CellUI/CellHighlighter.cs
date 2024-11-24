using UnityEngine;

namespace GameCore
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CellHighlighter : MonoBehaviour
    {
        [SerializeField]
        private CellsCollection _cellsCollection;

        [SerializeField]
        private GameObject _highlightImage;

        private BoxCollider2D _boxCollider;

        private InputSystem _inputSystem;

        private Vector3 _currentPos = new();

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
                _highlightImage.SetActive(true);

                var pos = _cellsCollection.GetCellCoordinates(mousePos);

                if (_currentPos == pos)
                {
                    return;
                }
                _highlightImage.transform.position = pos;

                _currentPos = pos;
            }
            else
            {
                _highlightImage.SetActive(false);
            }
        }
    }
}