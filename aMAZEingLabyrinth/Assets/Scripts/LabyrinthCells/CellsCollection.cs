using GameCore;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.LabyrinthCells
{
    public class CellsCollection : MonoBehaviour
    {
        [SerializeField]
        private Transform _view1;

        [SerializeField]
        private Transform _view2;

        private readonly List<(int X, int Y)> _angleCell = new()
        {
            (1, 0),
            (0, 1)
        };

        [SerializeField]
        private int _angleDeg = 90;

        [SerializeField]
        private bool _check = false;

        [SerializeField]
        private int _cellNumber = 1;

        private CardCell _cell1;
        private CardCell _cell2;

        private void Start()
        {
            _cell1 = new CardCell(_angleCell, _view1);
            _cell2 = new CardCell(_angleCell, _view2);

            Debug.Log("cell1:");
            _cell1.PrintMatrix();

            Debug.Log("cell2:");
            _cell2.PrintMatrix();
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