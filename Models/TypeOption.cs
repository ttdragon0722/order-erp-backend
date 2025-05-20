using System.Text.Json.Serialization;

namespace erp_server.Models
{
    public class TypeOption
    {
        public Guid TypeId { get; set; }
        [JsonIgnore]
        public TypeEntity? Type { get; set; }

        public Guid OptionId { get; set; }
        [JsonIgnore]
        public Option? Option { get; set; }
    }
}
