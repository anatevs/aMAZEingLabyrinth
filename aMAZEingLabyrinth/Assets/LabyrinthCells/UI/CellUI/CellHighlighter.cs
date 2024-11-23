using UnityEngine;

namespace GameCore
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class CellHighlighter : MonoBehaviour
    {
        [SerializeField]
        private GameObject _highlightImage;

        private BoxCollider2D _boxCollider;

        private InputSystem _inputSystem;

        private readonly float _halfLabyrinth = 21 / 2;
        private readonly int _halfCellsCoordinate = 18 / 2;
        private readonly int _cellSize = 3;

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
                var x = GetCellCenter(mousePos.x);
                var y = GetCellCenter(mousePos.y);

                var pos = new Vector2(x, y);

                _highlightImage.transform.position = pos;
            }
            else
            {
                _highlightImage.SetActive(false);
            }
        }

        private int GetCellCenter(float coordinate)
        {
            return ((int)(coordinate + _halfLabyrinth) / _cellSize) * _cellSize - _halfCellsCoordinate;
        }
    }
}