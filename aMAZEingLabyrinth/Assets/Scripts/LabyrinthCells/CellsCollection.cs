using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class CellsCollection : MonoBehaviour
    {
        [SerializeField]
        private CardCell[] _fixedCells;

        [SerializeField]
        private Transform _pathMarkersTransform;

        private LabyrinthGrid _grid;

        private int _cellSize = 3;

        private CardCellValues[,] _cardCells;

        private (int Rows, int Cols) _size = (7, 7);

        private int[] _fixedIndexes = new int[4] { 0, 2, 4, 6 };
        private int[] _movableIndexes = new int[3] { 1, 3, 5 };

        private int _tShapeCount = 6 + 12; //all are reward
        private int _angleShapeCount = 16 + 4; //6 are reward
        private int _lineShapeCount = 12;



        [SerializeField]
        private CardCell _view1;

        [SerializeField]
        private CardCell _view2;

        private CardCellValues _cell1;
        private CardCellValues _cell2;

        [SerializeField]
        private int _angleDeg = 90;

        [SerializeField]
        private bool _check = false;

        [SerializeField]
        private int _cellNumber = 1;

        [SerializeField]
        private GameObject _pathMarker;

        [SerializeField]
        private int[] _startPoint = new int[2];

        [SerializeField]
        private int[] _endPoint = new int[2];

        [SerializeField]
        private bool _findPath;

        [SerializeField]
        private int[] _printRowCol = new int[2];

        [SerializeField]
        private bool _printCell;

        private void RotateView(Transform view, int angleDeg)
        {
            view.rotation = CellRotationInfo.GetQuaternion90(angleDeg) * view.rotation;
        }

        private void Start()
        {
            _grid = new LabyrinthGrid((_size.Rows * _cellSize, _size.Cols * _cellSize));

            Debug.Log($"grid is within {_size.Rows * _cellSize} rows and {_size.Cols * _cellSize} columns");

            _cardCells = new CardCellValues[_size.Rows, _size.Cols];

            foreach (var cell in _fixedCells)
            {
                var (iCell, jCell) = GetCardIndex(((int)cell.transform.localPosition.x, (int)cell.transform.localPosition.y));

                _cardCells[iCell, jCell] =
                    new CardCellValues(cell.Geometry, cell.transform);

                SetCartToGridValues(iCell, jCell);
            }

            //_cell1 = new CardCellValues(CellGeometry.TShape, _view1.transform);
            //_cell2 = new CardCellValues(CellGeometry.Line, _view2);

            var otherCells = new CardCell[2] { _view1, _view2 };
            foreach (var cell in otherCells)
            {
                var (iCell, jCell) = GetCardIndex(((int)cell.transform.localPosition.x, (int)cell.transform.localPosition.y));

                _cardCells[iCell, jCell] =
                    new CardCellValues(cell.Geometry, cell.transform);

                SetCartToGridValues(iCell, jCell);
            }

            //make cicle to generate all game field

            //1. empty 7x7 with empty cells
            //2. fullfill static with values and views
            //3. randomly fullfill dynamic ones

            //var view = Instantiate(_view1);
            //view.position =
        }

        private void Update()
        {
            if (_check)
            {
                var cardIndex = (6, 1);

                if (_cellNumber == 2)
                {
                    cardIndex = (5, 2);
                }

                var rotatedCell = _cardCells[cardIndex.Item1, cardIndex.Item2];

                rotatedCell.Rotate(_angleDeg);

                SetCartToGridValues(cardIndex.Item1, cardIndex.Item2);

                _check = false;
            }


            if (_findPath)
            {
                for (int i = 0; i < _pathMarkersTransform.childCount; i++)
                {
                    Destroy(_pathMarkersTransform.GetChild(i).gameObject);
                }

                var xLocal = (int)transform.position.x;
                var yLocal = (int)transform.position.y;

                var start = new Vector2Int(_startPoint[0], _startPoint[1]);
                var end = new Vector2Int(_endPoint[0], _endPoint[1]);

                var res = _grid.TryFindAStarPath(start, end, out List<Vector2Int> result);

                var markerPos = new Vector3(_startPoint[0] + xLocal, _startPoint[1] + yLocal, _pathMarker.transform.position.z);

                Instantiate(_pathMarker, markerPos, Quaternion.identity, transform);

                markerPos.x = _endPoint[0] + xLocal;
                markerPos.y = _endPoint[1] + yLocal;
                Instantiate(_pathMarker, markerPos, Quaternion.identity, _pathMarkersTransform);

                if (res)
                {
                    Debug.Log($"path found: {res}");

                    foreach (var pathPoint in result)
                    {
                        markerPos.x = pathPoint.x + xLocal;
                        markerPos.y = pathPoint.y + yLocal;

                        Instantiate(_pathMarker, markerPos, Quaternion.identity, _pathMarkersTransform);
                    }
                }
                else
                {
                    Debug.Log($"path found: {res}");
                }

                _findPath = false;
            }

            if (_printCell)
            {
                var cell = _cardCells[_printRowCol[0], _printRowCol[1]];
                if (cell == null)
                {
                    Debug.Log("null cell");
                }
                else
                {
                    cell.PrintMatrix();
                }

                _printCell = false;
            }
        }

        private void SetCartToGridValues(int i, int j)
        {
            var card = _cardCells[i, j];

            for (int ic = 0; ic < card.Size; ic++)
            {
                for (int jc = 0; jc < card.Size; jc++)
                {
                    var xy = GetXY((i, j), (ic, jc));

                    _grid.SetValue(card.GetValue(ic, jc), xy);
                }
            }
        }

        private (int X, int Y) GetXY((int i, int j) cellIndex, (int ic, int jc) elementIndex)
        {
            var x = cellIndex.j * _cellSize + elementIndex.jc;
            var y = _size.Rows * _cellSize - 1 - (cellIndex.i * _cellSize + elementIndex.ic);
            return (x, y);
        }

        private (int iCell, int jCell) GetCardIndex((int x, int y) coordinates)
        {
            int i = _size.Rows - 1 - coordinates.y / _cellSize;
            int j = coordinates.x / _cellSize;

            return (i, j);
        }

        private (int iElement, int jElement) GetCardElementIndex((int x, int y) coordinates)
        {
            int ic = _cellSize - 1 - coordinates.y % _cellSize;
            int jc = coordinates.x % _cellSize;

            return (ic, jc);
        }
    }
}