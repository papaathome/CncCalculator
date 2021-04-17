using System.Collections.Generic;

using Newtonsoft.Json;
using System.IO;

namespace As.Tools.Data.Json
{
    /// <summary>
    /// transform: transform data structures between C# and Json domains.
    /// </summary>
    public static class Jtransform<T> where T : class
    {
        public static string SerializeObject(object value, bool isEmptyToNull = false, JsonSerializerSettings jsonSettings = null)
        {
            string response = string.Empty;
            if (value != null) response = JsonConvert.SerializeObject(value, jsonSettings);
            return (isEmptyToNull)
                ? (response == "{}") ? "null" : response
                : response;
        }

        public static T DeserializeObject(StreamReader stream, JsonSerializerSettings jsonSettings = null)
        {
            return DeserializeObject(stream.ReadToEnd(), jsonSettings);
        }

        public static T DeserializeObject(string value, JsonSerializerSettings jsonSettings = null)
        {
            T response = null;
            if (!string.IsNullOrEmpty(value))
            {
                response = (jsonSettings == null)
                    ? JsonConvert.DeserializeObject<T>(value)
                    : JsonConvert.DeserializeObject<T>(value, jsonSettings);
            }
            return response;
        }
    }
}
