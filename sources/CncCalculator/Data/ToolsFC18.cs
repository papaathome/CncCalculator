using System.Collections.Generic;
using System.IO;

using Newtonsoft.Json;
using As.Tools.Data.Json;

namespace As.Apps.Data
{
    class ToolFc18
    {
        [JsonProperty("version")]
        public int Version { get; set; }

        [JsonProperty("cornerRadius")]
        public double CornerRadius { get; set; }

        [JsonProperty("cuttingEdgeAngle")]
        public double CuttingEdgeAngle { get; set; }

        [JsonProperty("cuttingEdgeHeight")]
        public double CuttingEdgeHeight { get; set; }

        [JsonProperty("diameter")]
        public double Diameter { get; set; }

        [JsonProperty("flatRadius")]
        public double FlatRadius { get; set; }

        [JsonProperty("lengthOffset")]
        public double LengthOffset { get; set; }

        [JsonProperty("material")]
        public string Material { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("tooltype")]
        public string Tooltype { get; set; }
    }

    class ToolsFc18 : JsonBase<ToolsFc18>
    {
        #region Tools list
        const string DATA = @"{
  ""Tools"": {
    ""2"": {
      ""cornerRadius"": 0.0, 
      ""cuttingEdgeAngle"": 180.0, 
      ""cuttingEdgeHeight"": 20.0, 
      ""diameter"": 2.5, 
      ""flatRadius"": 2.5, 
      ""lengthOffset"": 0.0, 
      ""material"": ""HighSpeedSteel"", 
      ""name"": ""Endmill 2.50, 2 flute"", 
      ""tooltype"": ""EndMill"", 
      ""version"": 1
    }, 
    ""3"": {
      ""cornerRadius"": 0.0, 
      ""cuttingEdgeAngle"": 180.0, 
      ""cuttingEdgeHeight"": 20.0, 
      ""diameter"": 2.65, 
      ""flatRadius"": 2.65, 
      ""lengthOffset"": 0.0, 
      ""material"": ""HighSpeedSteel"", 
      ""name"": ""EndMill 2.65, 2 flute"", 
      ""tooltype"": ""EndMill"", 
      ""version"": 1
    }, 
    ""4"": {
      ""cornerRadius"": 0.0, 
      ""cuttingEdgeAngle"": 180.0, 
      ""cuttingEdgeHeight"": 24.0, 
      ""diameter"": 2.85, 
      ""flatRadius"": 2.85, 
      ""lengthOffset"": 0.0, 
      ""material"": ""HighSpeedSteel"", 
      ""name"": ""Endmill 2.85 1 flute"", 
      ""tooltype"": ""EndMill"", 
      ""version"": 1
    }, 
    ""5"": {
      ""cornerRadius"": 0.0, 
      ""cuttingEdgeAngle"": 180.0, 
      ""cuttingEdgeHeight"": 9.0, 
      ""diameter"": 10.0, 
      ""flatRadius"": 10.0, 
      ""lengthOffset"": 0.0, 
      ""material"": ""HighSpeedSteel"", 
      ""name"": ""Endmill Red 10, 2 flute"", 
      ""tooltype"": ""EndMill"", 
      ""version"": 1
    }, 
    ""6"": {
      ""cornerRadius"": 0.0, 
      ""cuttingEdgeAngle"": 180.0, 
      ""cuttingEdgeHeight"": 11.0, 
      ""diameter"": 16.0, 
      ""flatRadius"": 16.0, 
      ""lengthOffset"": 0.0, 
      ""material"": ""HighSpeedSteel"", 
      ""name"": ""Endmill Red 16, 2 flute"", 
      ""tooltype"": ""EndMill"", 
      ""version"": 1
    }, 
    ""7"": {
      ""cornerRadius"": 0.0, 
      ""cuttingEdgeAngle"": 180.0, 
      ""cuttingEdgeHeight"": 12.0, 
      ""diameter"": 20.0, 
      ""flatRadius"": 20.0, 
      ""lengthOffset"": 0.0, 
      ""material"": ""HighSpeedSteel"", 
      ""name"": ""Endmill Red 20, 2 flute"", 
      ""tooltype"": ""EndMill"", 
      ""version"": 1
    }, 
    ""8"": {
      ""cornerRadius"": 0.0, 
      ""cuttingEdgeAngle"": 180.0, 
      ""cuttingEdgeHeight"": 13.0, 
      ""diameter"": 25.0, 
      ""flatRadius"": 25.0, 
      ""lengthOffset"": 0.0, 
      ""material"": ""HighSpeedSteel"", 
      ""name"": ""Endmill Red 25, 2 flute"", 
      ""tooltype"": ""EndMill"", 
      ""version"": 1
    }, 
    ""9"": {
      ""cornerRadius"": 0.0, 
      ""cuttingEdgeAngle"": 180.0, 
      ""cuttingEdgeHeight"": 15.0, 
      ""diameter"": 30.0, 
      ""flatRadius"": 30.0, 
      ""lengthOffset"": 0.0, 
      ""material"": ""HighSpeedSteel"", 
      ""name"": ""Endmill Red 30, 2 flute"", 
      ""tooltype"": ""EndMill"", 
      ""version"": 1
    }, 
    ""10"": {
      ""cornerRadius"": 0.0, 
      ""cuttingEdgeAngle"": 180.0, 
      ""cuttingEdgeHeight"": 20.0, 
      ""diameter"": 2.0, 
      ""flatRadius"": 2.0, 
      ""lengthOffset"": 0.0, 
      ""material"": ""HighSpeedSteel"", 
      ""name"": ""Boor hout,  2 mm"", 
      ""tooltype"": ""Drill"", 
      ""version"": 1
    }
  }, 
  ""Version"": 1
}
";
        #endregion

        [JsonProperty("Version")]
        public int Version { get; set; }

        [JsonProperty("Tools")]
        public Dictionary<string, ToolFc18> Tools { get; set; } = new Dictionary<string, ToolFc18>();
    }
}
