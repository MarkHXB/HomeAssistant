using SubSystemComponent;
using System.Text.Json.Serialization;

namespace HomeAssistant.Lib.Features.Features
{
    public class Feature
    {
        public string Name { get; set; }
        public int Notifications { get; set; }
        [JsonIgnore]
        public Subsystem Subsystem { get; set; }
        public string Output { get; set; }
    }
}
