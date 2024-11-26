using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class PlayerView : MonoBehaviour
    {
        public event Action OnPathSet;

        [SerializeField]
        private Transform _transform;

        public void MoveThroughPath(List<(int x, int y)> path)
        {
            for (int i = path.Count - 1; i >= 0; i--)
            {
                MoveToPoint(path[i]);
            }

            OnPathSet?.Invoke();
        }

        public void MoveToPoint((int x, int y) point)
        {
            _transform.localPosition = new Vector2(point.x, point.y);
        }
    }
}