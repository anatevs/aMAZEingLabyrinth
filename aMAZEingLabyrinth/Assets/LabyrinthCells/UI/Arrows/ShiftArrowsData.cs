namespace GameCore
{
    public class ShiftArrowsData
    {
        public (int, int) DisabledIndex;

        public bool AreArrowsActive;

        public (int, int) InvalidIndex;

        public ShiftArrowsData((int, int) disabledIndex,
            bool areActive, (int, int) innvalidIndex)
        {
            DisabledIndex = disabledIndex;
            AreArrowsActive = areActive;
            InvalidIndex = innvalidIndex;
        }
    }
}