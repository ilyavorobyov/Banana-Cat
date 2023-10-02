
using Unity.VisualScripting;

namespace YG
{
    [System.Serializable]
    public class SavesYG
    {
        public int idSave;
        public bool isFirstSession = true;
        public string language = "ru";
        public bool promptDone;

        public bool IsAlreadyOpened = false;
        public int MaxScore = 0;
    }
}
