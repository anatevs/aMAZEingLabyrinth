using Cysharp.Threading.Tasks;
using DG.Tweening;
using GameCore;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class RewardsInfoViews : MonoBehaviour
    {
        [SerializeField]
        private RewardInfoView[] _views;

        [SerializeField]
        private PlayersSpritesConfig _playerTypesConfig;

        [SerializeField]
        private Image _activePlayerHighlight;

        [SerializeField]
        private float _scaleUpDown = 1.5f;

        [SerializeField]
        private float _scaleHalfDuration = 0.2f;

        private Vector3[] _highlightPos;

        private readonly Dictionary<PlayerType, RewardInfoView> _rewardsViews = new();

        private readonly Dictionary<PlayerType, Vector3> _playableHighlightPos = new();

        private void Awake()
        {
            _highlightPos = new Vector3[_views.Length];

            for (int i = 0; i < _views.Length; i++)
            {
                _highlightPos[i] = new Vector3(
                    _activePlayerHighlight.transform.localPosition.x,
                    _views[i].transform.localPosition.y,
                    _activePlayerHighlight.transform.localPosition.z);
            }
        }

        public void InitViews(ICollection<Player> activePlayers)
        {
            ResetAllViews();

            if (_views.Length < activePlayers.Count)
            {
                throw new Exception("rewards info views number less " +
                    "than active players types number");
            }

            int i = 0;
            foreach (var playerType in activePlayers)
            {
                _views[i].gameObject.SetActive(true);

                _views[i].SetPlayerImage(
                    _playerTypesConfig.GetPlayerSprite(playerType.Type));

                _rewardsViews.Add(playerType.Type, _views[i]);

                _playableHighlightPos.Add(playerType.Type, _highlightPos[i]);

                i++;
            }
        }

        public void SetTargetsInfo(Player player, RewardsConfig rewardsConfig)
        {
            try
            {
                _rewardsViews[player.Type].SetCurrentRewardSprite(
                    rewardsConfig.GetRewardSprite(player.CurrentTarget));
            }
            catch
            {
                _rewardsViews[player.Type].SetNoReward();
            }

            _rewardsViews[player.Type].SetRemainTargets(
                Mathf.Max(0, player.RemainTargetsCount - 1));
        }

        public async UniTask ScaleViewUpDown(Player player)
        {
            await _rewardsViews[player.Type].ScaleUpDown(_scaleUpDown, _scaleHalfDuration);
        }

        public void SetActiveHighlight(PlayerType playerType)
        {
            _activePlayerHighlight.transform.localPosition =
                _playableHighlightPos[playerType];
        }

        public async UniTask SmoothSetActiveHighlight(PlayerType playerType, float duration)
        {
            await _activePlayerHighlight.transform
                .DOLocalMove(_playableHighlightPos[playerType], duration)
                .AsyncWaitForCompletion();
        }

        private void ResetAllViews()
        {
            foreach (var view in _views)
            {
                view.gameObject.SetActive(false);
            }

            _rewardsViews.Clear();
            _playableHighlightPos.Clear();
        }
    }
}