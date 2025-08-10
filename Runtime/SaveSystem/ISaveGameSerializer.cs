namespace JohaToolkit.UnityEngine.SaveSystem
{
    public interface ISaveGameSerializer<TSaveData>
    {
        bool Save(TSaveData saveData, string savePath);
        bool Load(string savePath, out TSaveData saveData);
    }
}