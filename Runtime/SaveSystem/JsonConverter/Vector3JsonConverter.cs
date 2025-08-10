using System;
using Newtonsoft.Json;
using UnityEngine;

namespace JohaToolkit.UnityEngine.SaveSystem
{
    public class Vector3JsonConverter : JsonConverter<Vector3>
    {
        public override void WriteJson(JsonWriter writer, Vector3 value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.z);
            writer.WriteEndObject();
        }

        public override Vector3 ReadJson(JsonReader reader, Type objectType, Vector3 existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Vector3 result = Vector3.zero;
            
            if (reader.TokenType != JsonToken.StartObject) 
                return result;
            
            while (reader.Read())
            {
                if (reader.TokenType == JsonToken.EndObject)
                    break;
                if (reader.TokenType != JsonToken.PropertyName)
                    continue;
                string propertyName = (string)reader.Value;
                
                if (!reader.Read())
                    continue;
                
                switch (propertyName)
                {
                    case "x":
                        result.x = Convert.ToSingle(reader.Value);
                        break;
                    case "y":
                        result.y = Convert.ToSingle(reader.Value);
                        break;
                    case "z":
                        result.z = Convert.ToSingle(reader.Value);
                        break;
                }
            }

            return result;
        }
    }
}
