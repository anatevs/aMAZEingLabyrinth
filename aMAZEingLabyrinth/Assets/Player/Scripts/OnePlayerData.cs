using System;

namespace GameCore
{
    [Serializable]
    public struct OnePlayerData
    {
        public PlayerType Type;

        public bool IsPlaying;

        public RewardName[] RewardTargets;

        public Coordinates LabyrinthCoordinateStruct;

        public OnePlayerData(PlayerType type, bool isPlaying,
            RewardName[] rewardNames, int x, int y)
        {
            Type = type;
            IsPlaying = isPlaying;
            RewardTargets = rewardNames;
            LabyrinthCoordinateStruct.x = x;
            LabyrinthCoordinateStruct.y = y;
        }


        [Serializable]
        public struct Coordinates
        {
            public int x;
            public int y;
        }
    }
}