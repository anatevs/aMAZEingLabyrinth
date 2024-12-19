using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace GameCore
{
    public sealed class PlayerView : MonoBehaviour
    {
        [SerializeField]
        private Transform _transform;

        public async UniTask MoveThroughPath(List<(int x, int y)> path, float duration = 0.1f)
        {
            for (int i = path.Count - 1; i >= 0; i--)
            {
                await MoveToPoint(path[i], duration);
            }
        }

        public void MoveToPoint((int x, int y) point)
        {
            _transform.localPosition = new Vector2(point.x, point.y);
        }

        public async UniTask MoveToPoint((int x, int y) point, float duration)
        {
            var tween = _transform.DOLocalMove(new Vector2(point.x, point.y), duration);

            await tween.AsyncWaitForCompletion();
        }
    }
}