using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class Grid : MonoBehaviour
    {
        private bool[,] _elements;

        private int _rowsCount;
        private int _columnsCount;


        private void Start()
        {
            var v1 = new Vector2Int(0, 0);

            var v2 = new Vector2Int(0, 0);

            Debug.Log(v1 == v2);
        }

        public Grid(Vector2Int count)
        {
            _elements = new bool[count.x, count.y];

            _rowsCount = count.x;
            _columnsCount = count.y;
        }

        private bool IsValid(Vector2Int point)
        {
            if (point.x > _rowsCount || point.y > _columnsCount)
            {
                return false;
            }

            return _elements[point.x, point.y];
        }


        private readonly int[] _shifts = { -1, 1 };

        private List<Vector2Int> GetValidNeighbors(Vector2Int point)
        {
            var result = new List<Vector2Int>();

            for (int i = 0; i < _shifts.Length; i++)
            {
                for (int j = 0; j < _shifts.Length; j++)
                {
                    var neighbor = new Vector2Int { x = point.x + i, y = point.y + j };

                    if (IsValid(neighbor))
                    {
                        result.Add(neighbor);
                    }
                }
            }

            return result;
        }

        public List<Vector2Int> FindAStarPath(Vector2Int startPoint, Vector2Int endPoint)
        {
            List<Vector2Int> result = new();

            List<ANode> opened = new();
            List<ANode> closed = new();

            var currentPoint = startPoint;

            var currentData = new AStarData
            {
                Point = currentPoint,
                PathDistance = 0,
                TargetDistance = (endPoint - currentPoint).sqrMagnitude
            };

            var currentNode = new ANode() { Parent = default, SelfData = currentData };

            closed.Add(currentNode);

            while (true)
            {
                if (currentPoint != endPoint)
                {
                    var validNeighbors = GetValidNeighbors(currentPoint);

                    foreach (var neighbor in validNeighbors)
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

            result.Add(endPoint);
            currentNode = closed[closed.Count - 1];
            result.Add(currentNode.SelfData.Point);

            while (currentNode.SelfData.Point != startPoint)
            {
                currentNode = currentNode.Parent;
                result.Add(currentNode.SelfData.Point);
            }

            result.Add(startPoint);

            return result;
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