using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace GameCore
{
    public sealed class CellsCollection : MonoBehaviour
    {
        [SerializeField]
        private CardCell[] _fixedCells;

        [SerializeField]
        private Transform _pathMarkersTransform;

        [SerializeField]
        private Transform _movableParentTransform;

        [SerializeField]
        private CellPrefabsConfig _cellPrefabsConfig;

        [SerializeField]
        private MovableCellsConfig _movableCellsConfig;

        [SerializeField]
        private Transform _playableCardTransform;

        private LabyrinthGrid _grid;

        private readonly int _cellSize = 3;

        private CardCellValues[,] _cardCells;

        private (int Rows, int Cols) _size = (7, 7);

        private readonly int[] _fixedRowCols = new int[4] { 0, 2, 4, 6 };
        private readonly int[] _movableRowCols = new int[3] { 1, 3, 5 };

        [SerializeField]
        private GameObject _pathMarker;

        [Header("Rotation test")]
        [SerializeField]
        private int _angleDeg = 90;

        [SerializeField]
        private int[] _rotateCardIndex = new int[2];

        [SerializeField]
        private bool _check = false;


        [Header("Find path test")]
        [SerializeField]
        private int[] _startPoint = new int[2];

        [SerializeField]
        private int[] _endPoint = new int[2];

        [SerializeField]
        private bool _findPath;

        [Header("Print cell test")]
        [SerializeField]
        private int[] _printRowCol = new int[2];

        [SerializeField]
        private bool _printCell;

        private void Start()
        {
            _grid = new LabyrinthGrid((_size.Rows * _cellSize, _size.Cols * _cellSize));

            _cardCells = new CardCellValues[_size.Rows, _size.Cols];

            foreach (var cell in _fixedCells)
            {
                var (iCell, jCell) = GetCardIndex(((int)cell.transform.localPosition.x,
                    (int)cell.transform.localPosition.y));

                cell.InitCellValues();

                SetValuesToLabyrinth(cell.CellValues, iCell, jCell);
            }

            InitMovableCellsNewGame();
        }

        private void InitMovableCellsNewGame()
        {
            var movableAmount = _size.Rows * _size.Cols + 1 - _fixedCells.Length;

            if (_movableCellsConfig.Count != movableAmount)
            {
                throw new System.Exception("Movable cells count in config and in collection must be equal!");
            }

            (int Row, int Col)[] movableNumbers =
                new (int Row, int Col)[movableAmount - 1];

            int count = 0;
            foreach (var i in _movableRowCols)
            {
                for (int j = 0; j < _size.Cols; j++)
                {
                    movableNumbers[count] = (i, j);
                    count++;
                }
            }

            foreach (var i in _fixedRowCols)
            {
                foreach (var j in _movableRowCols)
                {
                    movableNumbers[count] = (i, j);
                    count++;
                }
            }

            var indexList = new List<int>(Enumerable
                .Range(0, movableAmount));

            int[] rotations = new int[4] { 0, 90, 180, 270 };

            var spawner = new CellSpawner(_cellPrefabsConfig);

            foreach (var (Row, Col) in movableNumbers)
            {
                var rotation = rotations[Random.Range(0, rotations.Length)];

                var randomIndex = indexList[Random.Range(0, indexList.Count)];

                var cellType = _movableCellsConfig.GetCardCellType(randomIndex);

                indexList.Remove(randomIndex);


                var (X, Y) = GetXY((Row, Col), (2, 0));

                var cell = spawner.SpawnCell(cellType.CellGeometry, cellType.Reward, rotation, X, Y, _movableParentTransform);

                SetValuesToLabyrinth(cell.CellValues, Row, Col);
            }

            var plCellType = _movableCellsConfig.GetCardCellType(indexList[0]);

            spawner.SpawnCell(plCellType.CellGeometry, plCellType.Reward, 0, 0, 0, _playableCardTransform);

        }


        private void Update()
        {
            if (_check)
            {
                var rotatedCell = _cardCells[_rotateCardIndex[0], _rotateCardIndex[1]];

                rotatedCell.Rotate(_angleDeg);

                SetCardToGridValues(_rotateCardIndex[0], _rotateCardIndex[1]);

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

        private void SetValuesToLabyrinth(CardCellValues cellValues, int i, int j)
        {
            _cardCells[i, j] = cellValues;

            SetCardToGridValues(i, j);
        }

        private void SetCardToGridValues(int i, int j)
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