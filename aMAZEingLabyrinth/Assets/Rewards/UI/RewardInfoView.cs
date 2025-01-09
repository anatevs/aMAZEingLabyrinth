using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameUI
{
    public class RewardInfoView : MonoBehaviour
    {
        [SerializeField]
        private Image _player;

        [SerializeField]
        private Image _currentRewardImage;

        [SerializeField]
        private TMP_Text _remainCardsText;

        [SerializeField]
        private Transform _scalePoint;

        public void SetPlayerImage(Sprite sprite)
        {
            _player.sprite = sprite;
        }

        public void SetCurrentRewardSprite(Sprite sprite)
        {
            _currentRewardImage.sprite = sprite;
        }

        public void SetNoReward()
        {
            _currentRewardImage.sprite = null;
        }

        public void SetRemainTargets(int remain)
        {
            if (remain == 0)
            {
                _remainCardsText.text = "";
                return;
            }

            _remainCardsText.text = remain.ToString();
        }

        public async UniTask ScaleUpDown(float scale, float halfDuration)
        {
            Sequence sequence = DOTween.Sequence().Pause();

            sequence.Append(_scalePoint.DOScale(scale, halfDuration)).Pause();
            sequence.Append(_scalePoint.DOScale(1, halfDuration)).Pause();

            await sequence.Play().AsyncWaitForCompletion();
        }
    }
}