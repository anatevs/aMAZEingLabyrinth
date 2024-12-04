using GameUI;
using UnityEngine;

namespace GameCore
{
    public sealed class PlayersList : MonoBehaviour
    {
        public Player CurrentPlayer => _players[_currentIndex];

        [SerializeField]
        private PlayersDataConfig _dataConfig;

        [SerializeField]
        private Player[] _players;

        [SerializeField]
        private RewardCardsService _rewardCardsService;

        private int _currentIndex = 0;

        private void OnDisable()
        {
            foreach (var player in _players)
            {
                _rewardCardsService.UnsubscribePlayers(player);

                player.OnSetPlaying -=
                    _rewardCardsService.SetActivePlayerHighlight;
            }
        }

        public void InitPlayers(PlayerType firstPlayer)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                var player = _players[i];

                player.Init(_dataConfig.GetData(player.Type));

                player.OnSetPlaying +=
                    _rewardCardsService.SetActivePlayerHighlight;

                if (firstPlayer == player.Type)
                {
                    player.SetIsPlaying(true);

                    _currentIndex = i;
                }
            }

            _rewardCardsService.DealOutCards(_players);
        }

        public void SetNextPlayer()
        {
            CurrentPlayer.SetIsPlaying(false);

            _currentIndex = (_currentIndex + 1) % _players.Length;

            CurrentPlayer.SetIsPlaying(true);
        }

        public void ReleasePlayerReward(int plIndex)
        {
            _players[plIndex].ReleaseReward();
        }
    }
}