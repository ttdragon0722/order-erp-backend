using System.Text.Json.Serialization;

namespace erp_server.Models
{
    public class MaterialTag
    {
        public Guid MaterialId { get; set; }
        [JsonIgnore]
        public virtual Material Material { get; set; } = null!;

        public Guid TagId { get; set; }
        [JsonIgnore]
        public virtual Tag Tag { get; set; } = null!;
    }
}
