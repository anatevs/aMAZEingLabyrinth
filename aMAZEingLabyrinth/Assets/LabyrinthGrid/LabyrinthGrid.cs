using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class LabyrinthGrid
    {
        private readonly int[,] _elements;

        private readonly int _xCount;
        private readonly int _yCount;

        public LabyrinthGrid((int X, int Y) count)
        {
            _elements = new int[count.X, count.Y];

            _xCount = count.X;
            _yCount = count.Y;
        }

        public bool IsValid((int X, int Y) point)
        {
            var x = point.X;
            var y = point.Y;

            if (x >= _xCount || y >= _yCount || x < 0 || y < 0)
            {
                return false;
            }

            return _elements[x, y] != 0;
        }

        public void SetValue(int value, (int X, int Y) point)
        {
            _elements[point.X, point.Y] = value;
        }

        private readonly int[] _neighborsShifts = { -1, 1 };

        private List<Vector2Int> GetValidNeighbors((int X, int Y) point)
        {
            var result = new List<Vector2Int>();

            for (int i = 0; i < _neighborsShifts.Length; i++)
            {
                var neighborPos = new Vector2Int { x = point.X + _neighborsShifts[i], y = point.Y };

                if (IsValid((neighborPos.x, neighborPos.y)))
                {
                    result.Add(neighborPos);
                }
            }

            for (int j = 0; j < _neighborsShifts.Length; j++)
            {
                var neighborPos = new Vector2Int { x = point.X, y = point.Y + _neighborsShifts[j] };

                if (IsValid((neighborPos.x, neighborPos.y)))
                {
                    result.Add(neighborPos);
                }
            }

            return result;
        }

        public bool TryFindAStarPath(Vector2Int startPoint, Vector2Int endPoint, out List<Vector2Int> result)
        {
            result = new();

            if (!(IsValid((startPoint.x, startPoint.y)) && IsValid((endPoint.x, endPoint.y))))
            {
                Debug.Log("start point or end point is not valid");
                return false;
            }

            if (startPoint == endPoint)
            {
                Debug.Log("start and end point is the same");
                result.Add(endPoint);

                return true;
            }

            List<ANode> opened = new();
            List<ANode> closed = new();
            List<int> markedPoints = new();

            var currentPoint = startPoint;

            var currentData = new AStarData
            {
                Point = currentPoint,
                PathDistance = 0,
                TargetDistance = (endPoint - currentPoint).sqrMagnitude
            };

            var currentNode = new ANode() { Parent = default, SelfData = currentData };

            closed.Add(currentNode);

            var currentXY = new int[2] { currentPoint.x, currentPoint.y };
            markedPoints.Add(currentXY[0]);
            markedPoints.Add(currentXY[1]);


            int baka = 1000;
            while (baka != 0)
            {
                baka--;

                if (currentPoint != endPoint)
                {
                    var validNeighbors = GetValidNeighbors((currentPoint.x, currentPoint.y));

                    foreach (var neighbor in validNeighbors)
                    {
                        currentXY[0] = neighbor.x;
                        currentXY[1] = neighbor.y;

                        if (!ContainsXY(markedPoints, currentXY))
                        {
                            var neighborData = new AStarData
                            {
                                Point = neighbor,
                                PathDistance = (currentData.PathDistance + 1) * (currentData.PathDistance + 1),
                                TargetDistance = (endPoint - neighbor).sqrMagnitude
                            };

                            var neighborNode = new ANode()
                            {
                                Parent = currentNode,
                                SelfData = neighborData
                            };

                            opened.Add(neighborNode);
                            markedPoints.Add(currentXY[0]);
                            markedPoints.Add(currentXY[1]);
                        }
                    }

                    if (opened.Count != 0)
                    {
                        var minDistance = opened[0].SelfData.SumDistance;
                        var nodeToClose = opened[0];
                        foreach (var openNode in opened)
                        {
                            var pointDistance = openNode.SelfData.SumDistance;
                            if (pointDistance < minDistance)
                            {
                                minDistance = pointDistance;
                                nodeToClose = openNode;
                            }
                        }

                        opened.Remove(nodeToClose);
                        closed.Add(nodeToClose);

                        currentNode = nodeToClose;

                        currentPoint = nodeToClose.SelfData.Point;
                    }
                    else
                    {
                        closed = new();
                        break;
                    }
                }
                else
                {
                    break;
                }
            }

            if (closed.Count > 1)
            {
                currentNode = closed[closed.Count - 1];
                result.Add(currentNode.SelfData.Point);

                while (currentNode.SelfData.Point != startPoint)
                {
                    currentNode = currentNode.Parent;
                    result.Add(currentNode.SelfData.Point);
                }
            }

            Debug.Log(baka);
            Debug.Log($"closed amount {closed.Count}, opened amount {opened.Count}");


            return (result.Count > 1);
        }

        private bool ContainsXY(List<int> check, int[] xy)
        {
            for (int i = 0; i < check.Count; i += 2)
            {
                if (check[i] == xy[0] && check[i+1] == xy[1])
                {
                    return true;
                }
            }

            return false;
        }
    }


    public class ANode
    {
        public ANode Parent;

        public AStarData SelfData;
    }

    public struct AStarData
    {
        public Vector2Int Point;

        public int PathDistance;

        public int TargetDistance;

        public int SumDistance => PathDistance + TargetDistance;
    }
}