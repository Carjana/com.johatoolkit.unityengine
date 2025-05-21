using System.Runtime.Serialization;

namespace JohaToolkit.UnityEngine.SaveSystem
{
    public interface ISaveGameUser
    {
        public void Save(SaveGame saveGame);
        public void Load(SaveGame saveGame);
    }
}
