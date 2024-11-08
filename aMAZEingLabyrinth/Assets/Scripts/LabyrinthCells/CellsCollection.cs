using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace GameCore
{
    public class CellsCollection : MonoBehaviour
    {
        [SerializeField]
        private Transform _view1;

        [SerializeField]
        private Transform _view2;

        private CardCell _cell1;
        private CardCell _cell2;

        private LabyrinthGrid _grid;

        private CardCell[,] _cardCells;


        private int _tShapeCount = 6 + 12; //all are reward + 12 staying reward
        private int _angleShapeCount = 16 + 4; //6 are reward + 4 staying
        private int _lineShapeCount = 12;

        private readonly List<(int X, int Y)> _angleCell = new()
        {
            (1, 0),
            (0, 1)
        };

        private readonly List<(int X, int Y)> _tCell = new()
        {
            (1, 0),
            (-1, 0),
            (0, 1)
        };

        private readonly List<(int X, int Y)> _lineCell = new()
        {
            (1, 0),
            (-1, 0)
        };



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


        

        private void Start()
        {
            _cell1 = new CardCell(_angleCell, _view1);
            _cell2 = new CardCell(_tCell, _view2);


            //Debug.Log("cell1:");
            //_cell1.PrintMatrix();

            //Debug.Log("cell2:");
            //_cell2.PrintMatrix();

            //make cicle to generate all game field

            //1. empty 7x7 with empty cells
            //2. fullfill static with values and views
            //3. randomly fullfill dynamic ones

            //var view = Instantiate(_view1);
            //view.position =

            _cardCells = new CardCell[,]
            {
                { _cell1, _cell2 }
            };

            var xSize = _cell1.Size * _cardCells.GetLength(1);
            var ySize = _cell1.Size * _cardCells.GetLength(0);

            _grid = new LabyrinthGrid((xSize, ySize));

            for (int i = 0;  i < _cardCells.GetLength(0); i++)
            {
                for (int j = 0; j < _cardCells.GetLength(1); j++)
                {
                    SetCardValues(i, j);
                }
            }
        }

        private void Update()
        {
            if (_check)
            {
                var rotatedCell = _cell1;
                var cardIndex = (0, 0);


                if (_cellNumber == 2)
                {
                    rotatedCell = _cell2;
                    cardIndex = (0, 1);
                }

                rotatedCell.Rotate(_angleDeg);

                //Debug.Log($"cell{_cellNumber} rotated to {_angleDeg}");
                //rotatedCell.PrintMatrix();

                SetCardValues(cardIndex.Item1, cardIndex.Item2);

                _check = false;
            }


            if (_findPath)
            {
                var xLocal = (int)transform.position.x;
                var yLocal = (int)transform.position.y;

                var start = new Vector2Int(_startPoint[0], _startPoint[1]);
                var end = new Vector2Int(_endPoint[0], _endPoint[1]);

                var res = _grid.TryFindAStarPath(start, end, out List<Vector2Int> result);

                var markerPos = new Vector3(_startPoint[0] + xLocal, _startPoint[1] + yLocal, _pathMarker.transform.position.z);

                Instantiate(_pathMarker, markerPos, Quaternion.identity, transform);

                markerPos.x = _endPoint[0] + xLocal;
                markerPos.y = _endPoint[1] + yLocal;
                Instantiate(_pathMarker, markerPos, Quaternion.identity, transform);

                //_labyrinthView.SetPathCell(start);
                //_labyrinthView.SetPathCell(end);

                if (res)
                {
                    Debug.Log($"path found: {res}");

                    foreach (var pathPoint in result)
                    {
                        markerPos.x = pathPoint.x + xLocal;
                        markerPos.y = pathPoint.y + yLocal;

                        Instantiate(_pathMarker, markerPos, Quaternion.identity, transform);
                        //var tilePoint = new Vector2Int(pathPoint.x - _centerShiftX, pathPoint.y - _centerShiftY);
                        //_labyrinthView.SetPathCell(tilePoint);
                        //Debug.Log(pathPoint);
                    }
                }
                else
                {
                    Debug.Log($"path found: {res}");
                }

                _findPath = false;
            }
        }

        private void SetCardValues(int i, int j)
        {
            var card = _cardCells[i, j];

            for (int ic = 0; ic < card.Size; ic++)
            {
                for (int jc = 0; jc < card.Size; jc++)
                {
                    var x = j * card.Size + jc;
                    var y = card.Size - 1 - (i * card.Size + ic);

                    _grid.SetValue(card.GetValue(ic, jc), (x, y));
                }
            }
        }
    }
}