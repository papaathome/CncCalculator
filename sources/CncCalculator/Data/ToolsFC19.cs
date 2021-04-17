using System.Collections.Generic;
using System.IO;
using System;

using Newtonsoft.Json;
using As.Tools.Data.Json;

namespace As.Apps.Data
{
    public class Bit : JsonBase<Bit>
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("shape")]
        public string Shape { get; set; }

        [JsonProperty("parameter")]
        public Dictionary<string, string> Parameter { get; set; }

        [JsonProperty("attribute")]
        public Dictionary<string, string> Attribute { get; set; }
    }

    public class Tool
    {
        [JsonProperty("nr")]
        public string Nr { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }
    }

    public class ToolsList
    {
#if USE_LOG4NET
        protected static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
#endif

        public static ToolsList GetData(string path, bool read_bits = true, JsonSerializerSettings jsonSettings = null)
        {
            string p = null;
            if (read_bits)
            {
                p = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(path)), "Bit");
            }
            using (var sr = new StreamReader(path)) return GetData(sr, p, jsonSettings);
        }

        public static ToolsList GetData(string path, string bits_path, JsonSerializerSettings jsonSettings = null)
        {
            using (var sr = new StreamReader(path)) return GetData(sr, bits_path, jsonSettings);
        }

        public static ToolsList GetData(StreamReader sr, string bits_path = null, JsonSerializerSettings jsonSettings = null)
        {
            var result = Deserialise(sr.ReadToEnd(), jsonSettings);
            if (!string.IsNullOrWhiteSpace(bits_path))
            {
                foreach (var t in result.Tools)
                {
                    string p = string.Empty;
                    try
                    {
                        p = Path.Combine(bits_path, t.Path);
                        var b = Bit.GetData(p);
                        result.Bits.Add(t.Nr, b);
                    }
                    catch (Exception x)
                    {
                        log.Debug($"ToolsList: parsing: '{p}'; problem: {x} ");
                    }
                }
            }
            return result;
        }

        public static ToolsList Deserialise(string value, JsonSerializerSettings jsonSettings = null)
        {
            return Jtransform<ToolsList>.DeserializeObject(value, jsonSettings);
        }

        [JsonProperty("tools")]
        public List<Tool> Tools { get; set; }

        [JsonProperty("version")]
        public int Version { get; set; }

        public Dictionary<string, Bit> Bits { get; set; } = new Dictionary<string, Bit>();
    }
}
