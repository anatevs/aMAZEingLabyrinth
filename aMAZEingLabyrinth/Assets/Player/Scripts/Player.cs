using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCore
{
    public sealed class Player : MonoBehaviour
    {
        //public event Action<(int, int)> OnSetCoordinate;

        public PlayerType Type => _playerType;

        public (int, int) Coordinate => _coordinate;

        public RewardName RewardTarget => _rewardTargets.Peek();

        [SerializeField]
        private PlayerView _view;

        [SerializeField]
        private PlayerType _playerType;

        private bool _isPlaying;

        private (int x, int j) _coordinate;

        private RewardName _rewardTarget;

        private Queue<RewardName> _rewardTargets;

        private CellsCollection _cellsCollection;

        public void Init(PlayerData data)
        {
            _isPlaying = data.IsPlaying;

            SetCoordinate((data.LabyrinthCoordinate.x, data.LabyrinthCoordinate.y));
        }

        public void SetIsPlaying(bool isPlaying)
        {
            _isPlaying = isPlaying;
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
        }

        public void SetRewards(RewardName[] rewards)
        {
            foreach(var target in rewards)
            {
                _rewardTargets.Enqueue(target);
            }
        }

        public void ReleaseReward()
        {
            _rewardTargets.Dequeue();
        }
    }
}