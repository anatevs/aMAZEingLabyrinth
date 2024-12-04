using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class Player : MonoBehaviour
    {
        public event Action OnMoved;

        public event Action<Player> OnTargetChanged;

        public event Action<PlayerType> OnSetPlaying;

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

        private bool _isPlaying;

        private (int x, int j) _coordinate;

        private readonly Queue<RewardName> _rewardTargets = new();

        public void Init(OnePlayerData data)
        {
            _isPlaying = data.IsPlaying;

            SetCoordinate((data.LabyrinthCoordinateStruct.x, data.LabyrinthCoordinateStruct.y));
        }

        public void SetIsPlaying(bool isPlaying)
        {
            _isPlaying = isPlaying;

            if (_isPlaying)
            {
                OnSetPlaying?.Invoke(_playerType);
            }
        }

        public void SetCoordinate((int x, int y) coordinate)
        {
            _coordinate = coordinate;

            _view.MoveToPoint(coordinate);
        }

        public void MoveThroughPath(List<(int x, int y)> path)
        {
            _coordinate = path[0];

            _view.MoveThroughPath(path);

            OnMoved?.Invoke();
        }

        public void AddReward(RewardName reward)
        {
            _rewardTargets.Enqueue(reward);
        }

        public void ReleaseReward()
        {
            _rewardTargets.Dequeue();

            OnTargetChanged?.Invoke(this);

            Debug.Log("last reward released");
            //PrintTargets();
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