using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cysharp.Threading.Tasks;

namespace GameCore
{
    public sealed class Player : MonoBehaviour
    {
        public PlayerType Type => _playerType;

        public (int X, int Y) Coordinate => _coordinate;

        public RewardName CurrentTarget => _rewardTargets.Peek();

        public bool IsPlaying => _isPlaying;

        public Queue<RewardName> RemainTargets => _rewardTargets;

        public int RemainTargetsCount => _rewardTargets.Count;

        [SerializeField]
        private PlayerView _view;

        [SerializeField]
        private PlayerType _playerType;

        [SerializeField]
        private float _oneMoveDuration = 0.05f;

        private bool _isPlaying;

        private (int x, int y) _coordinate;

        private (int x, int y) _defaultCoordinate;

        private readonly Queue<RewardName> _rewardTargets = new();

        public void Init(OnePlayerData data)
        {
            _isPlaying = data.IsPlaying;

            SetCoordinateAndView((data.LabyrinthCoordinateStruct.x, data.LabyrinthCoordinateStruct.y));

            SetupRewards(data.RewardTargets);
        }

        private void Awake()
        {
            var pos = _view.transform.localPosition;

            _defaultCoordinate = ((int)pos.x, (int)pos.y);
        }

        public void SetIsPlaying(bool isPlaying)
        {
            _isPlaying = isPlaying;
        }

        public void SetToDefaultPos()
        {
            SetCoordinateAndView(_defaultCoordinate);
        }

        public void SetCoordinateToDefaultPos()
        {
            _coordinate = _defaultCoordinate;
        }

        public void SetViewToDefaultPos()
        {
            _view.MoveToPoint(_defaultCoordinate);
        }

        public Tween PrepareViewSetToDefault(float duration)
        {
            return _view.PrepareMoveToPoint(_defaultCoordinate, duration);
        }

        public void SetCoordinateAndView((int x, int y) coordinate)
        {
            _coordinate = coordinate;

            _view.MoveToPoint(coordinate);
        }

        public void MoveThroughPath(List<(int x, int y)> path)
        {
            _coordinate = path[0];
        }

        public async UniTask MoveThroughPathVisual(List<(int x, int y)> path)
        {
            await _view.MoveThroughPath(path, _oneMoveDuration);
        }

        public void ShiftCoordinate(Vector3Int direction)
        {
            _coordinate = (_coordinate.x + direction.x, _coordinate.y + direction.y);
        }

        public Tween PrepareViewShift(Vector3Int direction, float duration)
        {
            return _view.PrepareShift(direction, duration);
        }

        public void AddReward(RewardName reward)
        {
            _rewardTargets.Enqueue(reward);
        }

        public void ReleaseReward()
        {
            _rewardTargets.Dequeue();
        }

        public void SetupRewards(RewardName[] rewardNames)
        {
            _rewardTargets.Clear();

            foreach (var rewardName in rewardNames)
            {
                _rewardTargets.Enqueue(rewardName);
            }
        }

        public void PrintTargets()
        {
            var str = "";

            Debug.Log($"{_playerType} has targets:");
            foreach (RewardName reward in _rewardTargets)
            {
                str += reward.ToString() + ", ";
            }

            Debug.Log(str);
            try
            {
                Debug.Log($"current target: {CurrentTarget}");
            }
            catch { Debug.Log("no more targets"); }
        }
    }
}