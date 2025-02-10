#define USE_LOG4NET

using System.IO;

using As.Applications.Data.Json;

using Newtonsoft.Json;

namespace As.Applications.Data
{
    public class Tool : JsonBase<Tool>
    {
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Name) ? "<<no name>>" : Name;
        }

        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = "";

        [JsonProperty("shape")]
        public string Shape { get; set; } = "";

        [JsonProperty("parameter")]
        public Dictionary<string, string> Parameter { get; set; } = [];

        [JsonProperty("attribute")]
        public Dictionary<string, string> Attribute { get; set; } = [];
    }

    public class ToolReference
    {
        [JsonProperty("nr")]
        public string Nr { get; set; } = "";

        [JsonProperty("path")]
        public string Path { get; set; } = "";
    }

    public class ToolsList
    {
#if USE_LOG4NET
        protected static readonly log4net.ILog Log
            = log4net.LogManager.GetLogger(nameof(ToolsList));
#endif

        public static ToolsList? GetData(
            string path,
            bool read_bits = true,
            JsonSerializerSettings? jsonSettings = null)
        {
            string p = "";
            if (read_bits)
            {
                var d = Path.GetDirectoryName(
                    Path.GetDirectoryName(path)) ?? ".";
                p = Path.Combine(d, "Bit");
            }
            using var sr = new StreamReader(path);
            return GetData(sr, p, jsonSettings);
        }

        public static ToolsList? GetData(
            string path,
            string bits_path,
            JsonSerializerSettings? jsonSettings = null)
        {
            if (!File.Exists(path)) return null;
            using var sr = new StreamReader(path);
            return GetData(sr, bits_path, jsonSettings);
        }

        public static ToolsList? GetData(
            StreamReader sr,
            string bits_path = "",
            JsonSerializerSettings? jsonSettings = null)
        {
            var result = Deserialise(sr.ReadToEnd(), jsonSettings);
            if (result == null) return null;

            if (!string.IsNullOrWhiteSpace(bits_path))
            {
                foreach (var t in result.ListOfToolReferences)
                {
                    string p = string.Empty;
                    try
                    {
                        p = Path.Combine(bits_path, t.Path);
                        var b = Tool.GetData(p);
                        if (b != null) result.Tools.Add(t.Nr, b);
                    }
#if USE_LOG4NET
                    catch (Exception x)
                    {
                        Log.ErrorFormat($"ToolsList: {x.Message.Trim()}; path = \"{p}\"");
                        for (
                            var i = x.InnerException;
                            i != null;
                            i = i.InnerException)
                        {
                            Log.ErrorFormat($"ToolsList: {x.Message.Trim()}");
                        }
                    }
#else
                    catch { }
#endif
                }
            }
            return result;
        }

        public static ToolsList? Deserialise(
            string value,
            JsonSerializerSettings? jsonSettings = null)
        {
            return Jtransform<ToolsList>.DeserializeObject(value, jsonSettings);
        }

        [JsonProperty("tools")]
        public List<ToolReference> ListOfToolReferences { get; set; } = [];

        [JsonProperty("version")]
        public int Version { get; set; }

        public Dictionary<string, Tool> Tools { get; set; } = [];
    }
}
