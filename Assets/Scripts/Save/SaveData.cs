using System;

namespace Match3.Save
{
    [Serializable]
    public class SaveData
    {
        public bool[] isActive;
        public int[] highScores;
        public int[] stars;
    }
}