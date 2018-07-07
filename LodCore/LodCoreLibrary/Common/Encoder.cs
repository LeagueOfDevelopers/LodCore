using Newtonsoft.Json;

namespace LodCoreLibrary.Common
{
    public static class Encoder
    {
        public static string Encode(dynamic obj)
        {
            string serializedObj = JsonConvert.SerializeObject(obj);
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(serializedObj);
            var encoded = System.Convert.ToBase64String(plainTextBytes);
            return encoded;
        }
    }
}
