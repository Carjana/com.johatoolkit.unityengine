namespace JohaToolkit.UnityEngine.SaveSystem
{
    public static class ObjectSerializer<TSaveData>
    {
        public static bool Save(ISaveGameSerializer<TSaveData> serializer, TSaveData saveData, string savePath)
        {
            return serializer.Save(saveData, savePath);
        }

        public static bool Load(ISaveGameSerializer<TSaveData> serializer, string savePath, out TSaveData saveData)
        {
            return serializer.Load(savePath, out saveData);
        }
    }
}
