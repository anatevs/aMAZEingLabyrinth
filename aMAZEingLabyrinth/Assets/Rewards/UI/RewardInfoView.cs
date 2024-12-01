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
            _remainCardsText.text = remain.ToString();
        }
    }
}