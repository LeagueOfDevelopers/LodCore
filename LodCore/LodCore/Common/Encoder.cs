using System;
using System.Text;
using Newtonsoft.Json;

namespace LodCore.Common
{
    public static class Encoder
    {
        public static string Encode(dynamic obj)
        {
            string serializedObj = JsonConvert.SerializeObject(obj);
            var plainTextBytes = Encoding.UTF8.GetBytes(serializedObj);
            var encoded = Convert.ToBase64String(plainTextBytes);
            return encoded;
        }
    }
}