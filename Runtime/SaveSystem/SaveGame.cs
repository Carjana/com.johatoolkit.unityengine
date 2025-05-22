using System.Collections.Generic;

namespace JohaToolkit.UnityEngine.SaveSystem
{
    [System.Serializable]
    public class SaveGame
    {
        public string SaveName { get; set; }
        public string SaveFilePath { get; set; }
        public Dictionary<string, object> SaveData { get; set; } = new();
    }
}
