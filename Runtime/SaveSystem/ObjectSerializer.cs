using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Newtonsoft.Json;
using UnityEngine;

namespace JohaToolkit.UnityEngine.SaveSystem
{
    public static class ObjectSerializer
    {
        private static readonly List<SurrogateInfo> Surrogates = new()
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
        
        public static bool SaveObject(object saveData, string savePath)
        {
            BinaryFormatter formatter = GetBinaryFormatter();
            
            if (!Directory.Exists(Path.GetDirectoryName(savePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(savePath));
            }
            
            FileStream fileStream = File.Create(savePath);

            try
            {
                formatter.Serialize(fileStream, saveData);
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to serialize data: " + e.Message);
                return false;
            }
            
            fileStream.Close();
            
            return true;
        }
        
        public static object LoadObject(string savePath)
        {
            BinaryFormatter formatter = GetBinaryFormatter();
            
            FileStream fileStream = File.Open(savePath, FileMode.Open);
            
            try
            {
                object saveData = formatter.Deserialize(fileStream);
                
                fileStream.Close();
                
                return saveData;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load save data: " + e.Message);
                fileStream.Close();
                return null;
            }

        }
        
        public static BinaryFormatter GetBinaryFormatter()
        {
            BinaryFormatter formatter = new();
            
            SurrogateSelector selector = new();
            foreach (SurrogateInfo surrogateInfo in Surrogates)
            {
                selector.AddSurrogate(surrogateInfo.Type, new StreamingContext(StreamingContextStates.All), surrogateInfo.Surrogate);
            }
            
            formatter.SurrogateSelector = selector;
            
            return formatter;
        }

        public static void AddSurrogate(Type type, ISerializationSurrogate surrogate)
        {
            Surrogates.Add(new SurrogateInfo()
            {
                Type = type,
                Surrogate = surrogate
            });
        }

        private struct SurrogateInfo
        {
            public Type Type;
            public ISerializationSurrogate Surrogate;
        }

        public static bool SaveJson(object obj, string savePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(obj, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented
                });
                File.WriteAllText(savePath, json);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to serialize and/or write data: " + e.Message);
                return false;
            }
        }

        public static T LoadJson<T>(string savePath) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(File.ReadAllText(savePath), new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.All,
                    Formatting = Formatting.Indented,
                });
            }
            catch (Exception e)
            {
                Debug.LogError("Failed to load JSON data: " + e.Message);
                return null;
            }
        }
    }
}
