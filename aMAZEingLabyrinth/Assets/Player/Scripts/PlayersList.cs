using SaveLoadNamespace;
using UnityEngine;
using VContainer;

namespace GameCore
{
    public sealed class PlayersList : MonoBehaviour
    {
        public Player CurrentPlayer => _players[_currentIndex];

        [SerializeField]
        private Player[] _players;

        [SerializeField]
        private RewardCardsService _rewardCardsService;

        private PlayersDataConnector _playersDataConnector;

        private int _currentIndex = 0;

        [Inject]
        public void Construct(PlayersDataConnector playersDataConnector)
        {
            _playersDataConnector = playersDataConnector;

            _playersDataConnector.OnPlayersRequested += SendPlayersToConnector;
        }

        private void OnDisable()
        {
            foreach (var player in _players)
            {
                _rewardCardsService.UnsubscribePlayers(player);

                player.OnSetPlaying -=
                    _rewardCardsService.SetActivePlayerHighlight;
            }

            _playersDataConnector.OnPlayersRequested -= SendPlayersToConnector;
        }

        public void InitPlayers(PlayerType firstPlayer)
        {
            for (int i = 0; i < _players.Length; i++)
            {
                var player = InitPlayer(i);

                if (firstPlayer == player.Type)
                {
                    player.SetIsPlaying(true);

                    _currentIndex = i;
                }
            }

            _rewardCardsService.DealOutDefaultCards(_players);
        }

        public void InitPlayers()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                var player = InitPlayer(i);

                if (player.IsPlaying)
                {
                    player.SetIsPlaying(true);

                    _currentIndex = i;
                }
            }

            _rewardCardsService.DealOutLoadedCards(_players);
        }

        private Player InitPlayer(int i)
        {
            var player = _players[i];

            player.Init(_playersDataConnector.Data.GetPlayerData(player.Type));

            player.OnSetPlaying +=
                _rewardCardsService.SetActivePlayerHighlight;

            return player;
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

        private void SendPlayersToConnector()
        {
            _playersDataConnector.SetPlayers(_players);
        }
    }
}