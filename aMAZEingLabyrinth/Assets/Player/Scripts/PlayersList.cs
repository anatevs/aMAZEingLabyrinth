using UnityEngine;

namespace GameCore
{
    public sealed class PlayersList : MonoBehaviour
    {
        public Player CurrentPlayer => _players[_currentIndex];

        public int PlayersCount => _players.Length;

        [SerializeField]
        private PlayersDataConfig _dataConfig;

        [SerializeField]
        private Player[] _players;

        [SerializeField]
        private RewardCardsService _rewardCardsService;

        private int _currentIndex = 0;

        private void Start()
        {
            InitPlayers(PlayerType.Blue);

            foreach (var player in _players)
            {
                player.OnMoved += SetNextPlayer;
            }

            _rewardCardsService.DealOutCards(this);
        }

        private void OnDisable()
        {
            foreach (var player in _players)
            {
                player.OnMoved -= SetNextPlayer;
            }
        }

        private void InitPlayers(PlayerType firstPlayer)
        {
            foreach (var player in _players)
            {
                player.Init(_dataConfig.GetData(player.Type));

                if (firstPlayer == player.Type)
                {
                    player.SetIsPlaying(true);
                }
            }
        }

        public void SetNextPlayer()
        {
            CurrentPlayer.SetIsPlaying(false);

            _currentIndex = (_currentIndex + 1) % _players.Length;

            CurrentPlayer.SetIsPlaying(true);
        }

        public void AddPlayerReward(int plIndex, RewardName reward)
        {
            _players[plIndex].AddReward(reward);
        }

        public void ReleasePlayerReward(int plIndex)
        {
            _players[plIndex].ReleaseReward();
        }
    }
}