using System;

namespace GameCore
{
    [Serializable]
    public struct PlayerData
    {
        public PlayerType Type;

        public bool IsPlaying;

        public RewardName[] RewardTargets;

        public Coordinates LabyrinthCoordinate;


        [Serializable]
        public struct Coordinates
        {
            public int x;
            public int y;
        }
    }
}