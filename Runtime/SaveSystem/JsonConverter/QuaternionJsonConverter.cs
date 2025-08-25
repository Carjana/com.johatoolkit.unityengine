using System;
using Newtonsoft.Json;
using UnityEngine;

namespace JohaToolkit.UnityEngine.SaveSystem.JsonConverter
{
    public class QuaternionJsonConverter : JsonConverter<Quaternion>
    {
        public override void WriteJson(JsonWriter writer, Quaternion value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("x");
            writer.WriteValue(value.x);
            writer.WritePropertyName("y");
            writer.WriteValue(value.y);
            writer.WritePropertyName("z");
            writer.WriteValue(value.z);
            writer.WritePropertyName("w");
            writer.WriteValue(value.w);
            writer.WriteEndObject();
        }

        public override Quaternion ReadJson(JsonReader reader, Type objectType, Quaternion existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            Quaternion result = Quaternion.identity;
            
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
                    case "w":
                        result.w = Convert.ToSingle(reader.Value);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(propertyName, "Unexpected property name when deserializing Quaternion");
                }
            }

            return result;
        }
    }
}