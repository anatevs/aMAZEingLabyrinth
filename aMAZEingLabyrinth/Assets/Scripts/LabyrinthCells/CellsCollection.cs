using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class CellsCollection : MonoBehaviour
    {
        [SerializeField]
        private Transform _view1;

        [SerializeField]
        private Transform _view2;

        private int _tShapeCount = 6; //all are reward
        private int _angleShapeCount = 16; //6 are reward
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
            (0, -1)
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

        private CardCell _cell1;
        private CardCell _cell2;

        private LabyrinthGrid _grid;

        private CardCell[,] _cardCells;

        private void Start()
        {
            _cell1 = new CardCell(_angleCell, _view1);
            _cell2 = new CardCell(_angleCell, _view2);

            Debug.Log("cell1:");
            _cell1.PrintMatrix();

            Debug.Log("cell2:");
            _cell2.PrintMatrix();

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
                    var card = _cardCells[i, j];

                    for (int ic = 0; ic < card.Values.GetLength(0); ic++)
                    {
                        for (int jc = 0; jc < card.Values.GetLength(1); jc++)
                        {
                            var x = j * card.Size + jc;
                            var y = i * card.Size + ic;
                        }
                    }
                }
            }

        }

        private void Update()
        {
            if (_check)
            {
                var rotatedCell = _cell1;

                if (_cellNumber == 2)
                {
                    rotatedCell = _cell2;
                }

                rotatedCell.Rotate(_angleDeg);

                Debug.Log($"cell{_cellNumber} rotated to {_angleDeg}");
                rotatedCell.PrintMatrix();

                _check = false;
            }
        }
    }
}