using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameCore
{
    [TestFixture]
    public sealed class LabyrinthGridTests
    {
        private LabyrinthGrid _labyrinthGrid;

        (int x, int y) _testSize;

        private readonly int[,] _tested = new int[5, 5]
        {
            { 1, 0, 0, 0, 0},
            { 1, 1, 1, 1, 0},
            { 0, 1, 0, 1, 0},
            { 0, 1, 0, 1, 0},
            { 1, 0, 1, 1, 1}
        };

        private bool[,] _testedValid;

        private Vector2Int[] _startEndOutrange;
        private Vector2Int[] _startEndCorrect;
        private Vector2Int _endUnreachable;
        private Vector2Int[] _startEndIncorrect;

        private List<Vector2Int> _correctPathPoints;

        [SetUp]
        public void Setup()
        {
            InitLabyrinthGrid();

            _startEndOutrange = new Vector2Int[2]
            {
                new Vector2Int(-1, 0),
                new Vector2Int(1, 5)
            };

            _startEndCorrect = new Vector2Int[2]
            {
                new Vector2Int(0, 0),
                new Vector2Int(4, 4)
            };

            _endUnreachable = new Vector2Int(4, 0);

            _startEndIncorrect = new Vector2Int[2]
            {
                new Vector2Int(0, 1),
                new Vector2Int(2, 0)
            };

            _correctPathPoints = new()
            {
                new Vector2Int(4, 4), new Vector2Int(4, 3),
                new Vector2Int(3, 3),
                new Vector2Int(2, 3),
                new Vector2Int(1, 3), new Vector2Int(1, 2), new Vector2Int(1, 1), new Vector2Int(1, 0),
                new Vector2Int(0, 0)
            };
        }

        [Test]
        public void LabyrinthValuesInitIsCorrect()
        {
            var result = true;
            for (int i = 0; i < _testSize.x; i++)
            {
                for (int j = 0; j < _testSize.y; j++)
                {
                    result = _labyrinthGrid.IsValid((i, j)) == _testedValid[i, j];

                    if (!result) break;
                }
                if (!result) break;
            }

            Assert.IsTrue(result);
        }

        [Test]
        public void FindPathISFalseWhenStartIndexOutRange()
        {
            Assert.IsFalse(FindPathResult(_startEndOutrange[0], _startEndCorrect[1]));
        }

        [Test]
        public void FindPathISFalseWhenEndIndexOutRange()
        {
            Assert.IsFalse(FindPathResult(_startEndCorrect[0], _startEndOutrange[1]));
        }

        [Test]
        public void FindPathIsFalseWhenStartValueIsIncorrect()
        {
            Assert.IsFalse(FindPathResult(_startEndIncorrect[0], _startEndCorrect[1]));
        }

        [Test]
        public void FindPathIsFalseWhenEndValueIsIncorrect()
        {
            Assert.IsFalse(FindPathResult(_startEndCorrect[0], _startEndIncorrect[1]));
        }

        [Test]
        public void FindPathIsTrueWhenCorrectPoints()
        {
            Assert.IsTrue(FindPathResult(_startEndCorrect[0], _startEndCorrect[1]));
        }

        [Test]
        public void FindPathIsFalseWhenEndIsUnreachable()
        {
            Assert.IsFalse(FindPathResult(_startEndCorrect[0], _endUnreachable));
        }


        [Test]
        public void FoundPathIsCorrect()
        {
            _labyrinthGrid.TryFindAStarPath(
                _startEndCorrect[0], _startEndCorrect[1],
                out List<Vector2Int> pathPoints);

            Assert.IsTrue(pathPoints.Count == _correctPathPoints.Count);
            Assert.IsTrue(Enumerable.SequenceEqual(pathPoints, _correctPathPoints));
        }

        private void InitLabyrinthGrid()
        {
            (int x, int y) _testSize = (_tested.GetLength(0), _tested.GetLength(1));

            _labyrinthGrid = new LabyrinthGrid(_testSize);

            _testedValid = new bool[_testSize.x, _testSize.y];

            for (int i = 0; i < _testSize.x; i++)
            {
                for (int j = 0; j < _testSize.y; j++)
                {
                    _labyrinthGrid.SetValue(_tested[i, j], (i, j));

                    _testedValid[i, j] = _tested[i, j] != 0;
                }
            }
        }

        private bool FindPathResult(Vector2Int startPoint, Vector2Int endPoint)
        {
            var result = _labyrinthGrid.TryFindAStarPath(startPoint, endPoint, out _);
            return result;
        }
    }
}