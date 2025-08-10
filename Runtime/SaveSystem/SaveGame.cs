using System.Collections.Generic;

namespace JohaToolkit.UnityEngine.SaveSystem
{
    [System.Serializable]
    public class SaveGame
    {
        public string SaveName { get; set; }
        public Dictionary<int, object> SaveData { get; set; } = new();
    }
}
