using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using Object = System.Object;

namespace JohaToolkit.UnityEngine.SaveSystem
{
    public class BinarySaveGameSerializer<TSaveData> : ISaveGameSerializer<TSaveData>
    {
        public struct SurrogateInfo
        {
            public Type Type;
            public ISerializationSurrogate Surrogate;
        }

        public List<SurrogateInfo> Surrogates { get; set; } = new()
        {
            new SurrogateInfo()
            {
                Type = typeof(Vector3),
                Surrogate = new SerializationSurrogates.Vector3SerializationSurrogate()
            },
            new SurrogateInfo()
            {
                Type = typeof(Quaternion),
                Surrogate = new SerializationSurrogates.QuaternionSerializationSurrogate()
            },
        };

        public void AddSurrogate(Type type, ISerializationSurrogate surrogate)
        {
            if(Surrogates.Select(s => s.Type).Contains(type))
            {
                Debug.LogWarning($"Surrogate for type {type} already exists. Skipping addition.");
                return;
            }
            Surrogates.Add(new SurrogateInfo()
            {
                Type = type,
                Surrogate = surrogate
            });
        }

        public bool Save(TSaveData saveData, string savePath)
        {
            BinaryFormatter formatter = GetBinaryFormatter();

            if (!Directory.Exists(Path.GetDirectoryName(savePath)))
            {
                Debug.LogError("Save path does not exist " + savePath);
                return false;
            }

            FileStream fileStream = File.Create(savePath);

            try
            {
                formatter.Serialize(fileStream, saveData);
                fileStream.Close();
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to serialize data: " + e.Message);
                fileStream.Close();
                return false;
            }
        }

        public bool Load(string savePath, out TSaveData saveData)
        {
            saveData = default;

            BinaryFormatter formatter = GetBinaryFormatter();

            FileStream fileStream = File.Open(savePath, FileMode.Open);

            try
            {
                Object data = formatter.Deserialize(fileStream);
            
                saveData = (TSaveData)data;

                fileStream.Close();

                if(saveData == null)
                    Debug.LogError("Deserialized data is null. Check if the save file is valid and matches the expected type.");
            
                return saveData != null;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load save data: " + e.Message);
                fileStream.Close();
                return false;
            }
        }

        private BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new();

            SurrogateSelector selector = new();
            foreach (SurrogateInfo surrogateInfo in Surrogates)
            {
                selector.AddSurrogate(surrogateInfo.Type, new StreamingContext(StreamingContextStates.All),
                    surrogateInfo.Surrogate);
            }

            formatter.SurrogateSelector = selector;

            return formatter;
        }
    }
}