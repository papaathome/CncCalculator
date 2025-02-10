using System.Globalization;
using System.IO;

using Newtonsoft.Json;

namespace As.Applications.Data.Json
{
    /// <summary>
    /// transform: transform data structures between C# and Json domains.
    /// </summary>
    public static class Jtransform<T> where T : class
    {
        public static string SerializeObject(
            object value,
            bool isEmptyToNull = false,
            JsonSerializerSettings? jsonSettings = null)
        {
            string response = string.Empty;
            if (value != null)
            {
                jsonSettings ??= new JsonSerializerSettings
                {
                    Culture = CultureInfo.InvariantCulture
                };
                response = JsonConvert.SerializeObject(value, jsonSettings);
            }
            return isEmptyToNull
                ? response == "{}" ? "null" : response
                : response;
        }

        public static T? DeserializeObject(
            StreamReader stream,
            JsonSerializerSettings? jsonSettings = null)
        {
            jsonSettings ??= new JsonSerializerSettings
            {
                Culture = CultureInfo.InvariantCulture
            };
            return DeserializeObject(stream.ReadToEnd(), jsonSettings);
        }

        public static T? DeserializeObject(
            string value,
            JsonSerializerSettings? jsonSettings = null)
        {
            T? response = null;
            if (!string.IsNullOrEmpty(value))
            {
                jsonSettings ??= new JsonSerializerSettings
                {
                    Culture = CultureInfo.InvariantCulture,
                };
                response = JsonConvert.DeserializeObject<T>(value, jsonSettings);
            }
            return response;
        }
    }
}
