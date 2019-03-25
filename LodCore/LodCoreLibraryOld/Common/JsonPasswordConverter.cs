using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LodCoreLibraryOld.Common
{
    public class JsonPasswordConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Password);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var password = jsonObject["Value"].ToString();
            return Password.FromPlainString(password);
        }
    }
}