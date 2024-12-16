using GameCore;
using VContainer;

namespace SaveLoadNamespace
{
    public class PlayersSaveLoad : SaveLoad<PlayersData, PlayersDataConnector>
    {
        protected override PlayersData ConvertDataToParams(PlayersDataConnector dataConnector)
        {
            dataConnector.SetupPlayers();

            var players = dataConnector.Players;

            var dataArray = new OnePlayerData[players.Count];

            for (int i = 0; i < players.Count; i++)
            {
                var player = players[i];

                dataArray[i] = new OnePlayerData(player.Type, 
                    player.IsPlaying, player.RemainTargets.ToArray(),
                    player.Coordinate.X, player.Coordinate.Y);
            }

            PlayersData result = new();
            result.SetPlayersData(dataArray);

            return result;
        }

        protected override void LoadDefault(IObjectResolver context)
        {
            var dataConnector = context.Resolve<PlayersDataConnector>();

            dataConnector.SetDefaultData();
        }

        protected override void SetupParamsData(PlayersData paramsData, IObjectResolver context)
        {
            var dataConnector = context.Resolve<PlayersDataConnector>();

            dataConnector.SetPlayersData(paramsData);
        }
    }
}