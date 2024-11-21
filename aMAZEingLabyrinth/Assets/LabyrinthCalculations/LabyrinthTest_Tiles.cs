using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public class LabyrinthTest_Tiles : MonoBehaviour
    {
        [SerializeField]
        private GridCellView_Tiles _labyrinthView;

        [SerializeField]
        private int[] _startPoint = new int[2];

        [SerializeField]
        private int[] _endPoint = new int[2];

        [SerializeField]
        private bool _findPath;

        private (int, int) _size = (22, 14);
        private int _centerShiftX = 11;
        private int _centerShiftY = 7;

        private LabyrinthGrid _labyrinthGrid;

        private void Start()
        {
            _labyrinthGrid = new LabyrinthGrid(_size);

            for (int i = -_centerShiftX; i < _centerShiftX; i++)
            {
                for (int j = -_centerShiftY; j < _centerShiftY; j++)
                {
                    var value = _labyrinthView.GetValue((i, j));

                    _labyrinthGrid.SetValue(value, (i+_centerShiftX, j+_centerShiftY));
                }
            }
        }

        private void Update()
        {
            if (_findPath)
            {
                var start = new Vector2Int(_startPoint[0], _startPoint[1]);
                var end = new Vector2Int(_endPoint[0], _endPoint[1]);

                var res = _labyrinthGrid.TryFindAStarPath(start, end, out List<Vector2Int> result);

                //_labyrinthView.SetPathCell(start);
                //_labyrinthView.SetPathCell(end);

                if (res)
                {
                    Debug.Log($"path found: {res}");

                    foreach (var pathPoint in result)
                    {
                        var tilePoint = new Vector2Int(pathPoint.x - _centerShiftX, pathPoint.y - _centerShiftY);
                        _labyrinthView.SetPathCell(tilePoint);
                        Debug.Log(pathPoint);
                    }
                }
                else
                {
                    Debug.Log($"path found: {res}");
                }
                
                _findPath = false;
            }
        }
    }
}