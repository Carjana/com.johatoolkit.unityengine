using System.Collections.Generic;
using log4net.Util;

namespace JohaToolkit.UnityEngine.SaveSystem
{
    [System.Serializable]
    public class SaveGame
    {
        public string saveName;
        public readonly Dictionary<string, object> SaveData = new();
    }
}
