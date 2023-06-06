using System;

namespace Data
{
    [Serializable]
    public class NoteSaveData
    {
        public bool[] notes;
        public NoteSaveData(bool[] notes)
        {
            this.notes = notes;
        }

        public NoteSaveData()
        {
            this.notes = new bool[9];
        }
    }
}