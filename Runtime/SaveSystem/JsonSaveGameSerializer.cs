using System;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace JohaToolkit.UnityEngine.SaveSystem
{
    public class JsonSaveGameSerializer<TSaveData> : ISaveGameSerializer<TSaveData>
    {
        public JsonSerializerSettings SerializerSettings { get; set; } = new()
        {
            TypeNameHandling = TypeNameHandling.All,
            Formatting = Formatting.Indented
        };
        
        public bool Save(TSaveData saveData, string savePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(saveData, SerializerSettings);
                File.WriteAllText(savePath, json);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to serialize and/or write data: " + e.Message);
                return false;
            }
        }

        public bool Load(string savePath, out TSaveData saveData)
        {
            saveData = default;
            try
            {
                string json = File.ReadAllText(savePath);
                saveData = JsonConvert.DeserializeObject<TSaveData>(json, SerializerSettings);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load JSON data: " + e.Message);
                return false;
            }
        }
        
    }
}