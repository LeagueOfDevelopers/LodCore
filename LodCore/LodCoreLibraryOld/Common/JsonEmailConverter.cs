using System;
using System.Net.Mail;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LodCoreLibraryOld.Common
{
    public class JsonEmailConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(MailAddress);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var address = jsonObject["address"].ToString();
            return new MailAddress(address);
        }
    }
}