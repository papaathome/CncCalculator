using As.Applications.Data.Json;

using Newtonsoft.Json;

namespace As.Applications.Data
{
    public class Material
    {
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(Description)
                ? "<<no description>>"
                : Description;
        }

        [JsonProperty("description")]
        public string Description { get; set; } = "";

        [JsonProperty("use cutting speed")]
        public bool UseCuttingSpeed { get; set; } = true;

        [JsonProperty("cutting speed")]
        public string CuttingSpeed { get; set; } = "1 mm/s";

        [JsonProperty("spindle speed")]
        public string SpindleSpeed { get; set; } = "1 rpm";

        [JsonProperty("feed per tooth")]
        public string FeedPerTooth { get; set; } = "1 mm/tooth";

        [JsonProperty("version")]
        public int Version { get; set; } = 1;
    }

    public class MaterialList : JsonBase<MaterialList>
    {
        [JsonProperty("content version")]
        public string ContentVersion { get; set; } = "0a";

        [JsonProperty("description")]
        public string Description { get; set; }
            = "Short description of this material list.";

        [JsonProperty("materials")]
        public Dictionary<string, Material> Materials { get; set; } = [];

        [JsonProperty("version")]
        public int Version { get; set; } = 1;
    }
}
